using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LifxCloud.NET;

namespace StreamLaunch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeLaunchPad();
            InitializeConfig();
        }

        Interface launchPadInterface;
        bool padConnected = false;
        Config config;

        private void InitializeLaunchPad()
        {
            launchPadInterface = new Interface();
            if (launchPadInterface.getConnectedLaunchpads().Length > 0)
            {
                OnLaunchpadConnected();
            }
            else 
            {
                lblStatus.Text = "Could not find a Launchpad!";
            }
        }

        private void InitializeConfig()
        {
            if (File.Exists(FileUtilities.GetConfigFilePath()))
            {
                config = FileUtilities.ReadFromBinaryFile<Config>(FileUtilities.GetConfigFilePath());
                if (config != null && config.LifxToken != null)
                {
                    txtLifxToken.Text = config.LifxToken;
                    InitializeLifx();
                }
            }
            else
            {
                config = new Config();
            }
            
        }

        async private void InitializeLifx()
        {
            var client = await LifxCloudClient.CreateAsync(config.LifxToken);
            var lights = await client.ListLights();
        }

        private void OnLaunchpadConnected()
        {
            if (padConnected) return;

            Random random = new Random();

            launchPadInterface.connect(launchPadInterface.getConnectedLaunchpads()[0]); //Connects with your Launchpad
            for (int i = 0; i< 8; i++) {
                for (int j = 0; j < 8; j++) {
                    launchPadInterface.setLED(i, j, random.Next(1, 127));
                    System.Threading.Thread.Sleep(20);
                    launchPadInterface.setLED(i, j, 0);
                }
            }
            
            launchPadInterface.createTextScroll("Hello!", 100, false, 127);
            

            launchPadInterface.OnLaunchpadKeyPressed += OnLaunchpadKeyPressed;
            launchPadInterface.OnLaunchpadCCKeyPressed += OnLaunchpadCCKeyPressed;
            launchPadInterface.OnLaunchpadKeyDown += OnLaunchpadKeyDown;

            if (launchPadInterface.IsLegacy)
                lblStatus.Text = "Found Legacy Launchpad!";
            else
                lblStatus.Text = "Found Launchpad!";

            padConnected = true;
        }

        private void OnLaunchpadKeyPressed(object sender, Interface.LaunchpadKeyEventArgs e)
        {
           
        }

        private void OnLaunchpadKeyDown(object sender, Interface.LaunchpadKeyEventArgs e)
        {
            char rowLetter = (char)((char)'A' + (e.GetX()));

            string buttonName = "btn" + rowLetter + (e.GetY() + 1);

            Button pressed = (Button)this.Controls[buttonName];

            pressed.BackColor = Color.White;
            launchPadInterface.setLED(e.GetX(), e.GetY(), 127);
            System.Threading.Thread.Sleep(50);
            pressed.BackColor = SystemColors.ControlLight;
            launchPadInterface.setLED(e.GetX(), e.GetY(), 0);
        }

        private void OnLaunchpadCCKeyPressed(object sender, Interface.LaunchpadCCKeyEventArgs e) 
        {
            string buttonName = "btnSide" + (e.GetVal() + 1);
 
            Button pressed = (Button)this.Controls[buttonName];
  
            pressed.BackColor = Color.White;
            System.Threading.Thread.Sleep(50);
            pressed.BackColor = SystemColors.ControlLight;
        }

        /*
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (!padConnected) 
            {
                while (launchPadInterface.getConnectedLaunchpads().Length == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                OnLaunchpadConnected();
            }
        }*/

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Console.WriteLine("Closing!!!!");
            lblStatus.Text = "Finishing up...";
            
            if (launchPadInterface.getConnectedLaunchpads().Length > 0)
            {
                Console.WriteLine("Here!!!!");
                launchPadInterface.clearAllLEDs();
                launchPadInterface.disconnect(launchPadInterface.getConnectedLaunchpads()[0]);
            }
            
            base.OnFormClosing(e);
        }

        private void btnTop1_Click(object sender, EventArgs e)
        {

        }

        private void btnTop2_Click(object sender, EventArgs e)
        {

        }

        private void btnTop3_Click(object sender, EventArgs e)
        {

        }

        private void btnTop4_Click(object sender, EventArgs e)
        {

        }

        private void btnTop5_Click(object sender, EventArgs e)
        {

        }

        private void btnTop6_Click(object sender, EventArgs e)
        {

        }

        private void btnTop7_Click(object sender, EventArgs e)
        {

        }

        private void btn_Top8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void btnSide7_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateToken_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)this.Controls["txtLifxToken"];
            if (textBox.Text.Length > 0)
            {
                config.LifxToken = textBox.Text;
                FileUtilities.WriteToBinaryFile<Config>(FileUtilities.GetConfigFilePath(), config);
            }
        }
    }
}
