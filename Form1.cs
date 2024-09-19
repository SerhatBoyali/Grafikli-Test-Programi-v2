using Modbus.Device;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System;
using ZedGraph;
using System.Xml;
using OfficeOpenXml;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Linq;
namespace Grafik_Test_Programı
{
    public partial class Form1 : Form
    {
        private IModbusSerialMaster master;
        private SerialPort port;
        private GraphPane myPane1, myPane2, myPane3, myPane4, myPane5, myPane6, myPane7, myPane8, myPane9, myPane10;
        private PointPairList[] pointPairLists1, pointPairLists2, pointPairLists3, pointPairLists4, pointPairLists5, pointPairLists6, pointPairLists7, pointPairLists8, pointPairLists9, pointPairLists10;
        private LineItem[] curves1, curves2, curves3, curves4, curves5, curves6, curves7, curves8, curves9, curves10;
        private int time1, time2, time3, time4, time5, time6, time7, time8, time9, time10;
        private TextBox[] textBoxes1, textBoxes2, textBoxes3, textBoxes4, textBoxes5, textBoxes6, textBoxes7, textBoxes8, textBoxes9, textBoxes10;
        private TimeSpan elapsedTime1;
        private TimeSpan elapsedTime2;
        private TimeSpan elapsedTime3;
        private TimeSpan elapsedTime4;
        private TimeSpan elapsedTime5;
        private TimeSpan elapsedTime6;
        private TimeSpan elapsedTime7;
        private TimeSpan elapsedTime8;
        private TimeSpan elapsedTime9;
        private TimeSpan elapsedTime10;
        private Stopwatch stopwatch10;
        private Stopwatch stopwatch9;
        private Stopwatch stopwatch8;
        private Stopwatch stopwatch7;
        private Stopwatch stopwatch6;
        private Stopwatch stopwatch5;
        private Stopwatch stopwatch4;
        private Stopwatch stopwatch3;
        private Stopwatch stopwatch2;
        private Stopwatch stopwatch1;
        


        public Form1()
        {
            InitializeComponent();
            InitializeGraph1();
            InitializeGraph2();
            InitializeGraph3();
            InitializeGraph4();
            InitializeGraph5();
            InitializeGraph6();
            InitializeGraph7();
            InitializeGraph8();
            InitializeGraph9();
            InitializeGraph10();
            InitializeTimersAndLabels();

            string[] ports = SerialPort.GetPortNames();
            comboBoxPorts.Items.AddRange(ports);
            if (comboBoxPorts.Items.Count > 0)
            {
                comboBoxPorts.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Seri Port Dönüştürücüsünü takın.");
            }

            string[] baudRates = { "9600", "19200", "38400", "57600", "115200" };
            comboBoxBaudRate.Items.AddRange(baudRates);
            comboBoxBaudRate.SelectedIndex = 0;

            if (comboBoxPorts.SelectedItem != null)
            {
                InitializeModbus(comboBoxPorts.SelectedItem.ToString(), int.Parse(comboBoxBaudRate.SelectedItem.ToString()));
            }
            else
            {
                MessageBox.Show("No serial ports available. Please connect a serial port and try again.");
            }

            buttonRefresh.Click += new EventHandler(buttonRefresh_Click);
        }

        private void InitializeTimersAndLabels()
        {
            elapsedTime1 = new TimeSpan(0);
            elapsedTime2 = new TimeSpan(0);
            elapsedTime3 = new TimeSpan(0);
            elapsedTime4 = new TimeSpan(0);
            elapsedTime5 = new TimeSpan(0);
            elapsedTime6 = new TimeSpan(0);
            elapsedTime7 = new TimeSpan(0);
            elapsedTime8 = new TimeSpan(0);
            elapsedTime9 = new TimeSpan(0);
            elapsedTime10 = new TimeSpan(0);
        }

        private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPort = comboBoxPorts.SelectedItem.ToString();

            InitializeModbus(selectedPort, 9600);
        }
        private void comboBoxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedBaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());

            string selectedPort = comboBoxPorts.SelectedItem.ToString();

            InitializeModbus(selectedPort, selectedBaudRate);
        }
        private void InitializeModbus(string portName, int baudRate)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    port.Close();
                }

                port = new SerialPort(portName);
                port.BaudRate = baudRate;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;

                port.ReadTimeout = 2000;
                port.WriteTimeout = 2000;

                if (!port.IsOpen)
                {
                    port.Open();
                }
                master = ModbusSerialMaster.CreateRtu(port);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }
        private void DisconnectModbus()
        {
            try
            {
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer4.Stop();
                timer31.Stop();
                timer32.Stop();
                timer41.Stop();
                timer42.Stop();
                timer51.Stop();
                timer52.Stop();
                timer61.Stop();
                timer62.Stop();
                timer71.Stop();
                timer72.Stop();
                timer81.Stop();
                timer82.Stop();
                timer91.Stop();
                timer92.Stop();
                timer101.Stop();
                timer102.Stop();
                if (port != null && port.IsOpen)
                {
                    port.Close();
                    labelPortStatus.Text = "Port Bağlantısı: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Port kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /*    private void buttonStartSaving1_Click(object sender, EventArgs e)
            {
                SaveToExcel(1, pointPairLists1, time1, true);  // Manuel kaydetme (dosya yolu sorulur)
                SaveToExcel(1, pointPairLists1, time1, false); // Otomatik kaydetme (masaüstüne kaydedilir)
            }

            private void buttonStartSaving2_Click(object sender, EventArgs e)
            {
                SaveToExcel(2, pointPairLists2, time2, true);
                SaveToExcel(2, pointPairLists2, time2, false);
            }

            private void buttonStartSaving3_Click(object sender, EventArgs e)
            {
                SaveToExcel(3, pointPairLists3, time3, true);
                SaveToExcel(3, pointPairLists3, time3, false);
            }

            private void buttonStartSaving4_Click(object sender, EventArgs e)
            {
                SaveToExcel(4, pointPairLists4, time4, true);
                SaveToExcel(4, pointPairLists4, time4, false);
            }

            private void buttonStartSaving5_Click(object sender, EventArgs e)
            {
                SaveToExcel(5, pointPairLists5, time5, true);
                SaveToExcel(5, pointPairLists5, time5, false);
            }

            private void buttonStartSaving6_Click(object sender, EventArgs e)
            {
                SaveToExcel(6, pointPairLists6, time6, true);
                SaveToExcel(6, pointPairLists6, time6, false);
            }

            private void buttonStartSaving7_Click(object sender, EventArgs e)
            {
                SaveToExcel(7, pointPairLists7, time7, true);
                SaveToExcel(7, pointPairLists7, time7, false);
            }

            private void buttonStartSaving8_Click(object sender, EventArgs e)
            {
                SaveToExcel(8, pointPairLists8, time8, true);
                SaveToExcel(8, pointPairLists8, time8, false);
            }

            private void buttonStartSaving9_Click(object sender, EventArgs e)
            {
                SaveToExcel(9, pointPairLists9, time9, true);
                SaveToExcel(9, pointPairLists9, time9, false);
            }

            private void buttonStartSaving10_Click(object sender, EventArgs e)
            {
                SaveToExcel(10, pointPairLists10, time10, true);
                SaveToExcel(10, pointPairLists10, time10, false);
            } */



        private async void buttonStartSaving1_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync1(1, pointPairLists1, time1, cancellationTokenSource.Token);
        }
        private async void buttonStartSaving2_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync2(2, pointPairLists2, time2, cancellationTokenSource.Token);
        }

        private async void buttonStartSaving3_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync3(3, pointPairLists3, time3, cancellationTokenSource.Token);
        }

        private async void buttonStartSaving4_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync4(4, pointPairLists4, time4, cancellationTokenSource.Token);
        }

        private async void buttonStartSaving5_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync5(5, pointPairLists5, time5, cancellationTokenSource.Token);
        }

        private async void buttonStartSaving6_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync6(6, pointPairLists6, time6, cancellationTokenSource.Token);
        }
        private async void buttonStartSaving7_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync7(7, pointPairLists7, time7, cancellationTokenSource.Token);
        }

        private async void buttonStartSaving8_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync8(8, pointPairLists8, time8, cancellationTokenSource.Token);
        }


        private async void buttonStartSaving9_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync9(9, pointPairLists9, time9, cancellationTokenSource.Token);
        }

        private async void buttonStartSaving10_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SaveToExcelAsync10(10, pointPairLists10, time10, cancellationTokenSource.Token);
        }



        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string selectedPort = comboBoxPorts.SelectedItem.ToString();
            int selectedBaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());

            InitializeModbus(selectedPort, selectedBaudRate);

            if (port != null && port.IsOpen)
            {
                labelPortStatus.Text = "Port Bağlantısı: Açık";
            }
            else
            {
                labelPortStatus.Text = "Port Bağlantısı: Kapalı";
            }

            try
            {
                MessageBox.Show("Bağlantı başarılı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBaglantiyiKes_Click(object sender, EventArgs e)
        {
            DisconnectModbus();

            try
            {
                MessageBox.Show("Bağlantı kesildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }



        private CancellationTokenSource cancellationTokenSource;
        private void buttonConnect1_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer1.Start();
                    timer3.Start();
                    stopwatch1.Start();
                    labelConnectionStatus1.Text = "Bağlantı Durumu: Açık";
                    isDisconnected1 = false;
                }
                else
                {
                    timer1.Stop();
                    timer3.Stop();
                    stopwatch1.Stop();
                    labelConnectionStatus1.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }
        private void buttonDisconnect1_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer1.Stop();
                    timer3.Stop();
                    stopwatch1.Stop();

                    labelConnectionStatus1.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }
        private void buttonConnect2_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer2.Start();
                    timer4.Start();
                    stopwatch2.Start();
                    labelConnectionStatus2.Text = "Bağlantı Durumu: Açık";
                    isDisconnected2 = false;
                }
                else
                {
                    timer2.Stop();
                    timer4.Stop();
                    stopwatch2.Stop();
                    labelConnectionStatus2.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }
        private void buttonDisconnect2_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer2.Stop();
                    timer4.Stop();
                    stopwatch2.Stop();

                    labelConnectionStatus2.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }
        private void buttonConnect3_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer31.Start();
                    timer32.Start();
                    stopwatch3.Start();
                    labelConnectionStatus3.Text = "Bağlantı Durumu: Açık";
                    isDisconnected3 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer31.Stop();
                    timer32.Stop();
                    stopwatch3.Stop();
                    labelConnectionStatus3.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }
        private void buttonConnect4_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer41.Start();
                    timer42.Start();
                    stopwatch4.Start();
                    labelConnectionStatus4.Text = "Bağlantı Durumu: Açık";
                    isDisconnected4 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer41.Stop();
                    timer42.Stop();
                    stopwatch4.Stop();
                    labelConnectionStatus4.Text = "Bağlantı Durumu: Port Bağlı Değil";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private void buttonConnect5_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer51.Start();
                    timer52.Start();
                    stopwatch5.Start();
                    labelConnectionStatus5.Text = "Bağlantı Durumu: Açık";
                    isDisconnected5 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer51.Stop();
                    timer52.Stop();
                    stopwatch5.Stop();
                    labelConnectionStatus5.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private void buttonConnect6_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer61.Start();
                    timer62.Start();
                    stopwatch6.Start();
                    labelConnectionStatus6.Text = "Bağlantı Durumu: Açık";
                    isDisconnected6 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer61.Stop();
                    timer62.Stop();
                    stopwatch6.Stop();
                    labelConnectionStatus6.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private void buttonConnect7_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer71.Start();
                    timer72.Start();
                    stopwatch7.Start();
                    labelConnectionStatus7.Text = "Bağlantı Durumu: Açık";
                    isDisconnected7 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer71.Stop();
                    timer72.Stop();
                    stopwatch7.Stop();
                    labelConnectionStatus7.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private void buttonConnect9_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer91.Start();
                    timer92.Start();
                    stopwatch9.Start();
                    labelConnectionStatus9.Text = "Bağlantı Durumu: Açık";
                    isDisconnected9 = false;  // Port yeniden bağlandı, bayrak sıfırlanır
                }
                else
                {
                    timer91.Stop();
                    timer92.Stop();
                    stopwatch9.Stop();
                    labelConnectionStatus9.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private bool isDisconnected1 = false;
        private bool isDisconnected2 = false;
        private bool isDisconnected3 = false;
        private bool isDisconnected4 = false;
        private bool isDisconnected5 = false;
        private bool isDisconnected6 = false;
        private bool isDisconnected7 = false;
        private bool isDisconnected8 = false;
        private bool isDisconnected9 = false;
        private bool isDisconnected10 = false;

        private void CheckSerialPortStatus1()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer1.Stop();
                timer3.Stop();

                isDisconnected1 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus2()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer2.Stop();
                timer4.Stop();

                isDisconnected2 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus3()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer31.Stop();
                timer32.Stop();

                isDisconnected3 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus4()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer41.Stop();
                timer42.Stop();

                isDisconnected4 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus5()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer51.Stop();
                timer52.Stop();

                isDisconnected5 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus6()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer61.Stop();
                timer62.Stop();

                isDisconnected6 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus7()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer71.Stop();
                timer72.Stop();

                isDisconnected7 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus8()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer81.Stop();
                timer82.Stop();

                isDisconnected10 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus9()
        {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer81.Stop();
                timer82.Stop();

                isDisconnected8 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CheckSerialPortStatus10() {
            if (port == null || !port.IsOpen && !isDisconnected9)
            {
                timer101.Stop();
                timer102.Stop();

                isDisconnected10 = true;
                MessageBox.Show("Seri port bağlantısı kayboldu. Lütfen tekrar bağlanın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonConnect10_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer101.Start();
                    timer102.Start();
                    stopwatch10.Start();
                    labelConnectionStatus10.Text = "Bağlantı Durumu: Açık";
                    isDisconnected10 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer101.Stop();
                    timer102.Stop();
                    stopwatch10.Stop();
                    labelConnectionStatus10.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private void buttonDisconnect3_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer31.Stop();
                    timer32.Stop();
                    stopwatch3.Stop();

                    labelConnectionStatus3.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }

        private void buttonDisconnect8_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer81.Stop();
                    timer82.Stop();
                    stopwatch8.Stop();

                    labelConnectionStatus8.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }

        private void buttonDisconnect10_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer101.Stop();
                    timer102.Stop();
                    stopwatch10.Stop();

                    labelConnectionStatus10.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }

        private void buttonDisconnect9_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer91.Stop();
                    timer92.Stop();
                    stopwatch9.Stop();

                    labelConnectionStatus9.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }

        private void buttonDisconnect7_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer71.Stop();
                    timer72.Stop();
                    stopwatch7.Stop();

                    labelConnectionStatus9.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }

        private void buttonConnect8_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer81.Start();
                    timer82.Start();
                    stopwatch8.Start();
                    labelConnectionStatus7.Text = "Bağlantı Durumu: Açık";
                    isDisconnected8 = false;
                }
                if (port == null || !port.IsOpen)
                {
                    timer81.Stop();
                    timer82.Stop();
                    stopwatch8.Stop();
                    labelConnectionStatus7.Text = "Bağlantı Durumu: Port Bağlı Değil";
                    MessageBox.Show("Seri port açık değil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port açılırken hata oluştu: {ex.Message}");
            }
        }

        private void timer102_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime10 = stopwatch10.Elapsed;
            labelTimer10.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime10.Days, elapsedTime10.Hours, elapsedTime10.Minutes, elapsedTime10.Seconds);
        }

        private void timer101_Tick(object sender, EventArgs e)
        {
            ReadModbusData(10, pointPairLists10, ref time10, zedGraphControl10, new TextBox[] { textBox73, textBox74, textBox75, textBox76, textBox77, textBox78, textBox79, textBox80, textBoxWatt10 });
            UpdateAllTimers();
        }

        private void timer92_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime9 = stopwatch9.Elapsed;
            labelTimer9.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime9.Days, elapsedTime9.Hours, elapsedTime9.Minutes, elapsedTime9.Seconds);
        }
        private void timer91_Tick(object sender, EventArgs e)
        {
            try
            {
                ReadModbusData(9, pointPairLists9, ref time9, zedGraphControl9, new TextBox[] { textBox65, textBox66, textBox67, textBox68, textBox69, textBox70, textBox71, textBox72, textBoxWatt9 });
                UpdateAllTimers();
                CheckSerialPortStatus9();
            }
            catch (Exception ex)
            {
                timer91.Stop();
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }



        private void timer82_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime8 = stopwatch8.Elapsed;
            labelTimer8.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime8.Days, elapsedTime8.Hours, elapsedTime8.Minutes, elapsedTime8.Seconds);
        }

        private void timer81_Tick(object sender, EventArgs e)
        {
            ReadModbusData(8, pointPairLists8, ref time8, zedGraphControl8, new TextBox[] { textBox57, textBox58, textBox59, textBox60, textBox61, textBox62, textBox63, textBox64, textBoxWatt8 });
            UpdateAllTimers();
        }

        private void timer72_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime7 = stopwatch7.Elapsed;
            labelTimer7.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime7.Days, elapsedTime7.Hours, elapsedTime7.Minutes, elapsedTime7.Seconds);
        }

        private void timer71_Tick(object sender, EventArgs e)
        {
            ReadModbusData(7, pointPairLists7, ref time7, zedGraphControl7, new TextBox[] { textBox49, textBox50, textBox51, textBox52, textBox53, textBox54, textBox55, textBox56, textBoxWatt7 });
            UpdateAllTimers();
        }

        private void timer62_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime6 = stopwatch6.Elapsed;
            labelTimer6.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime6.Days, elapsedTime6.Hours, elapsedTime6.Minutes, elapsedTime6.Seconds);
        }

        private void timer61_Tick(object sender, EventArgs e)
        {
            ReadModbusData(6, pointPairLists6, ref time6, zedGraphControl6, new TextBox[] { textBox41, textBox42, textBox43, textBox44, textBox45, textBox46, textBox47, textBox48, textBoxWatt6 });
            UpdateAllTimers();
        }

        private void timer52_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime5 = stopwatch5.Elapsed;
            labelTimer5.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime5.Days, elapsedTime5.Hours, elapsedTime5.Minutes, elapsedTime5.Seconds);
        }

        private void timer51_Tick(object sender, EventArgs e)
        {
            ReadModbusData(5, pointPairLists5, ref time5, zedGraphControl5, new TextBox[] { textBox33, textBox34, textBox35, textBox36, textBox37, textBox38, textBox39, textBox40, textBoxWatt5 });
            UpdateAllTimers();
        }

        private void timer32_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime3 = stopwatch3.Elapsed;
            labelTimer3.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime3.Days, elapsedTime3.Hours, elapsedTime3.Minutes, elapsedTime3.Seconds);
        }

        private void timer31_Tick(object sender, EventArgs e)
        {
            ReadModbusData(3, pointPairLists3, ref time3, zedGraphControl3, new TextBox[] { textBox17, textBox18, textBox19, textBox20, textBox21, textBox22, textBox23, textBox24, textBoxWatt3 });
            UpdateAllTimers();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime2 = stopwatch2.Elapsed;
            labelTimer2.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime2.Days, elapsedTime2.Hours, elapsedTime2.Minutes, elapsedTime2.Seconds);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime1 = stopwatch1.Elapsed;
            labelTimer1.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime1.Days, elapsedTime1.Hours, elapsedTime1.Minutes, elapsedTime1.Seconds);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ReadModbusData(1, pointPairLists1, ref time1, zedGraphControl1, new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBoxWatt1 });
            UpdateAllTimers();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ReadModbusData(2, pointPairLists2, ref time2, zedGraphControl2, new TextBox[] { textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15, textBox16, textBoxWatt2 });
            UpdateAllTimers();
        }

        private void timer42_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime4 = stopwatch4.Elapsed;
            labelTimer4.Text = String.Format("{0:D2}Gün {1:D2}Saat {2:D2}Dakika {3:D2}Saniye ",
                elapsedTime4.Days, elapsedTime4.Hours, elapsedTime4.Minutes, elapsedTime4.Seconds);
        }

        private void timer41_Tick(object sender, EventArgs e)
        {
            ReadModbusData(4, pointPairLists4, ref time4, zedGraphControl4, new TextBox[] { textBox25, textBox26, textBox27, textBox28, textBox29, textBox30, textBox31, textBox32, textBoxWatt4 });
            UpdateAllTimers();
        }

        private void buttonDisconnect4_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer41.Stop();
                    timer42.Stop();
                    stopwatch4.Stop();

                    labelConnectionStatus4.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }
        private void buttonDisconnect5_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer51.Stop();
                    timer52.Stop();
                    stopwatch5.Stop();

                    labelConnectionStatus5.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }

        private void buttonDisconnect6_Click(object sender, EventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    timer61.Stop();
                    timer62.Stop();
                    stopwatch6.Stop();

                    labelConnectionStatus6.Text = "Bağlantı Durumu: Kapalı";
                    MessageBox.Show("Bağlantı kesildi.");
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    MessageBox.Show("Seri port zaten kapalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesilirken hata oluştu: {ex.Message}");
            }
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void buttonOut_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void InitializeGraph1()
        {
            myPane1 = zedGraphControl1.GraphPane;
            myPane1.XAxis.Title.Text = "Zaman";
            myPane1.YAxis.Title.Text = "Değer";
            myPane1.Title.Text = "Test Grafiği - ID 1";

            pointPairLists1 = new PointPairList[9];
            curves1 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists1[0] = new PointPairList();
            curves1[0] = myPane1.AddCurve("VOLTAJ", pointPairLists1[0], colors[0], SymbolType.None);

            pointPairLists1[1] = new PointPairList();
            curves1[1] = myPane1.AddCurve("AKIM", pointPairLists1[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists1[i] = new PointPairList();
                curves1[i] = myPane1.AddCurve(ntcNames[i - 2], pointPairLists1[i], colors[i], SymbolType.None);
            }

            // Watt verisi için ekleme
            pointPairLists1[8] = new PointPairList();
            curves1[8] = myPane1.AddCurve("WATT", pointPairLists1[8], colors[8], SymbolType.None);

            curves1[0].Line.Width = 4.0f; // VOLTAJ için kalınlık
            curves1[1].Line.Width = 4.0f; // AKIM için kalınlık
            for (int i = 2; i < 8; i++)
            {
                curves1[i].Line.Width = 2.5f; // NTC verileri için kalınlık
            }
            curves1[8].Line.Width = 3.0f; // Watt verisi için kalınlık

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }


        private void InitializeGraph2()
        {
            myPane2 = zedGraphControl2.GraphPane;
            myPane2.XAxis.Title.Text = "Zaman";
            myPane2.YAxis.Title.Text = "Değer";
            myPane2.Title.Text = "Test Grafiği - ID 2";

            pointPairLists2 = new PointPairList[9];
            curves2 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists2[0] = new PointPairList();
            curves2[0] = myPane2.AddCurve("VOLTAJ", pointPairLists2[0], colors[0], SymbolType.None);

            pointPairLists2[1] = new PointPairList();
            curves2[1] = myPane2.AddCurve("AKIM", pointPairLists2[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists2[i] = new PointPairList();
                curves2[i] = myPane2.AddCurve(ntcNames[i - 2], pointPairLists2[i], colors[i], SymbolType.None);
            }

            pointPairLists2[8] = new PointPairList();
            curves2[8] = myPane2.AddCurve("WATT", pointPairLists2[8], colors[8], SymbolType.None);

            curves2[0].Line.Width = 4.0f;
            curves2[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves2[i].Line.Width = 2.5f;
            }
            curves2[8].Line.Width = 3.0f;
            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();
        }
        private void InitializeGraph3()
        {
            myPane3 = zedGraphControl3.GraphPane;
            myPane3.XAxis.Title.Text = "Zaman";
            myPane3.YAxis.Title.Text = "Değer";
            myPane3.Title.Text = "Test Grafiği - ID 3";

            pointPairLists3 = new PointPairList[9];
            curves3 = new LineItem[9];
            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists3[0] = new PointPairList();
            curves3[0] = myPane3.AddCurve("VOLTAJ", pointPairLists3[0], colors[0], SymbolType.None);

            pointPairLists3[1] = new PointPairList();
            curves3[1] = myPane3.AddCurve("AKIM", pointPairLists3[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists3[i] = new PointPairList();
                curves3[i] = myPane3.AddCurve(ntcNames[i - 2], pointPairLists3[i], colors[i], SymbolType.None);
            }
            pointPairLists3[8] = new PointPairList();
            curves3[8] = myPane3.AddCurve("WATT", pointPairLists3[8], colors[8], SymbolType.None);

            curves3[0].Line.Width = 4.0f;
            curves3[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves3[i].Line.Width = 2.5f;
            }
            curves3[8].Line.Width = 3.0f;

            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();
        }


        private void InitializeGraph4()
        {
            myPane4 = zedGraphControl4.GraphPane;
            myPane4.XAxis.Title.Text = "Zaman";
            myPane4.YAxis.Title.Text = "Değer";
            myPane4.Title.Text = "Test Grafiği - ID 4";

            pointPairLists4 = new PointPairList[9];
            curves4 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists4[0] = new PointPairList();
            curves4[0] = myPane4.AddCurve("VOLTAJ", pointPairLists4[0], colors[0], SymbolType.None);

            pointPairLists4[1] = new PointPairList();
            curves4[1] = myPane4.AddCurve("AKIM", pointPairLists4[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists4[i] = new PointPairList();
                curves4[i] = myPane4.AddCurve(ntcNames[i - 2], pointPairLists4[i], colors[i], SymbolType.None);
            }
            pointPairLists4[8] = new PointPairList();
            curves4[8] = myPane4.AddCurve("WATT", pointPairLists4[8], colors[8], SymbolType.None);

            curves4[0].Line.Width = 4.0f;
            curves4[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves4[i].Line.Width = 2.5f;
            }
            curves4[8].Line.Width = 3.0f;

            zedGraphControl4.AxisChange();
            zedGraphControl4.Invalidate();
        }

        private void InitializeGraph5()
        {
            myPane5 = zedGraphControl5.GraphPane;
            myPane5.XAxis.Title.Text = "Zaman";
            myPane5.YAxis.Title.Text = "Değer";
            myPane5.Title.Text = "Test Grafiği - ID 5";

            pointPairLists5 = new PointPairList[9];
            curves5 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists5[0] = new PointPairList();
            curves5[0] = myPane5.AddCurve("VOLTAJ", pointPairLists5[0], colors[0], SymbolType.None);

            pointPairLists5[1] = new PointPairList();
            curves5[1] = myPane5.AddCurve("AKIM", pointPairLists5[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists5[i] = new PointPairList();
                curves5[i] = myPane5.AddCurve(ntcNames[i - 2], pointPairLists5[i], colors[i], SymbolType.None);
            }

            pointPairLists5[8] = new PointPairList();
            curves5[8] = myPane5.AddCurve("WATT", pointPairLists5[8], colors[8], SymbolType.None);

            curves5[0].Line.Width = 4.0f;
            curves5[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves5[i].Line.Width = 2.5f;
            }
            curves5[8].Line.Width = 3.0f;

            zedGraphControl5.AxisChange();
            zedGraphControl5.Invalidate();
        }

        private void InitializeGraph6()
        {
            myPane6 = zedGraphControl6.GraphPane;
            myPane6.XAxis.Title.Text = "Zaman";
            myPane6.YAxis.Title.Text = "Değer";
            myPane6.Title.Text = "Test Grafiği - ID 6";

            pointPairLists6 = new PointPairList[9];
            curves6 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists6[0] = new PointPairList();
            curves6[0] = myPane6.AddCurve("VOLTAJ", pointPairLists6[0], colors[0], SymbolType.None);

            pointPairLists6[1] = new PointPairList();
            curves6[1] = myPane6.AddCurve("AKIM", pointPairLists6[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists6[i] = new PointPairList();
                curves6[i] = myPane6.AddCurve(ntcNames[i - 2], pointPairLists6[i], colors[i], SymbolType.None);
            }

            pointPairLists6[8] = new PointPairList();
            curves6[8] = myPane6.AddCurve("WATT", pointPairLists6[8], colors[8], SymbolType.None);

            curves6[0].Line.Width = 4.0f;
            curves6[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves6[i].Line.Width = 2.5f;
            }
            curves6[8].Line.Width = 3.0f;

            zedGraphControl6.AxisChange();
            zedGraphControl6.Invalidate();
        }

        private void InitializeGraph7()
        {
            myPane7 = zedGraphControl7.GraphPane;
            myPane7.XAxis.Title.Text = "Zaman";
            myPane7.YAxis.Title.Text = "Değer";
            myPane7.Title.Text = "Test Grafiği - ID 7";

            pointPairLists7 = new PointPairList[9];
            curves7 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists7[0] = new PointPairList();
            curves7[0] = myPane7.AddCurve("VOLTAJ", pointPairLists7[0], colors[0], SymbolType.None);

            pointPairLists7[1] = new PointPairList();
            curves7[1] = myPane7.AddCurve("AKIM", pointPairLists7[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists7[i] = new PointPairList();
                curves7[i] = myPane7.AddCurve(ntcNames[i - 2], pointPairLists7[i], colors[i], SymbolType.None);
            }

            pointPairLists7[8] = new PointPairList();
            curves7[8] = myPane7.AddCurve("WATT", pointPairLists7[8], colors[8], SymbolType.None);

            curves7[0].Line.Width = 4.0f;
            curves7[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves7[i].Line.Width = 2.5f;
            }
            curves7[8].Line.Width = 3.0f;

            zedGraphControl7.AxisChange();
            zedGraphControl7.Invalidate();
        }

        private void InitializeGraph8()
        {
            myPane8 = zedGraphControl8.GraphPane;
            myPane8.XAxis.Title.Text = "Zaman";
            myPane8.YAxis.Title.Text = "Değer";
            myPane8.Title.Text = "Test Grafiği - ID 8";

            pointPairLists8 = new PointPairList[9];
            curves8 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists8[0] = new PointPairList();
            curves8[0] = myPane8.AddCurve("VOLTAJ", pointPairLists8[0], colors[0], SymbolType.None);

            pointPairLists8[1] = new PointPairList();
            curves8[1] = myPane8.AddCurve("AKIM", pointPairLists8[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists8[i] = new PointPairList();
                curves8[i] = myPane8.AddCurve(ntcNames[i - 2], pointPairLists8[i], colors[i], SymbolType.None);
            }

            pointPairLists8[8] = new PointPairList();
            curves8[8] = myPane8.AddCurve("WATT", pointPairLists8[8], colors[8], SymbolType.None);

            curves8[0].Line.Width = 4.0f;
            curves8[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves8[i].Line.Width = 2.5f;
            }
            curves8[8].Line.Width = 3.0f;

            zedGraphControl8.AxisChange();
            zedGraphControl8.Invalidate();
        }

        private void InitializeGraph9()
        {
            myPane9 = zedGraphControl9.GraphPane;
            myPane9.XAxis.Title.Text = "Zaman";
            myPane9.YAxis.Title.Text = "Değer";
            myPane9.Title.Text = "Test Grafiği - ID 9";

            pointPairLists9 = new PointPairList[9];
            curves9 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists9[0] = new PointPairList();
            curves9[0] = myPane9.AddCurve("VOLTAJ", pointPairLists9[0], colors[0], SymbolType.None);

            pointPairLists9[1] = new PointPairList();
            curves9[1] = myPane9.AddCurve("AKIM", pointPairLists9[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists9[i] = new PointPairList();
                curves9[i] = myPane9.AddCurve(ntcNames[i - 2], pointPairLists9[i], colors[i], SymbolType.None);
            }

            pointPairLists9[8] = new PointPairList();
            curves9[8] = myPane9.AddCurve("WATT", pointPairLists9[8], colors[8], SymbolType.None);

            curves9[0].Line.Width = 4.0f;
            curves9[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves9[i].Line.Width = 2.5f;
            }
            curves9[8].Line.Width = 3.0f;

            zedGraphControl9.AxisChange();
            zedGraphControl9.Invalidate();
        }

        private void InitializeGraph10()
        {
            myPane10 = zedGraphControl10.GraphPane;
            myPane10.XAxis.Title.Text = "Zaman";
            myPane10.YAxis.Title.Text = "Değer";
            myPane10.Title.Text = "Test Grafiği - ID 10";

            pointPairLists10 = new PointPairList[9];
            curves10 = new LineItem[9];

            Color[] colors = { Color.Blue, Color.Red, Color.Green, Color.Purple, Color.Orange, Color.Brown, Color.Pink, Color.Gray, Color.Cyan };

            pointPairLists10[0] = new PointPairList();
            curves10[0] = myPane10.AddCurve("VOLTAJ", pointPairLists10[0], colors[0], SymbolType.None);

            pointPairLists10[1] = new PointPairList();
            curves10[1] = myPane10.AddCurve("AKIM", pointPairLists10[1], colors[1], SymbolType.None);

            string[] ntcNames = { "NTC1", "NTC2", "NTC3", "NTC4", "NTC5", "NTC6" };
            for (int i = 2; i < 8; i++)
            {
                pointPairLists10[i] = new PointPairList();
                curves10[i] = myPane10.AddCurve(ntcNames[i - 2], pointPairLists10[i], colors[i], SymbolType.None);
            }

            pointPairLists10[8] = new PointPairList();
            curves10[8] = myPane10.AddCurve("WATT", pointPairLists10[8], colors[8], SymbolType.None);

            curves10[0].Line.Width = 4.0f;
            curves10[1].Line.Width = 4.0f;
            for (int i = 2; i < 8; i++)
            {
                curves10[i].Line.Width = 2.5f;
            }
            curves10[8].Line.Width = 3.0f;

            zedGraphControl10.AxisChange();
            zedGraphControl10.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            stopwatch10 = new Stopwatch();
            stopwatch9 = new Stopwatch();
            stopwatch8 = new Stopwatch();
            stopwatch7 = new Stopwatch();
            stopwatch6 = new Stopwatch();
            stopwatch5 = new Stopwatch();
            stopwatch4 = new Stopwatch();
            stopwatch3 = new Stopwatch();
            stopwatch2 = new Stopwatch();
            stopwatch1 = new Stopwatch();
        }


        private bool isConnectionLost = false; // Bağlantı kaybını izlemek için
        private bool hasShownConnectionLostMessage = false; // Mesajın gösterilip gösterilmediğini izlemek için

        private void ReadModbusData(byte slaveId, PointPairList[] pointPairLists, ref int time, ZedGraphControl zedGraphControl, TextBox[] textBoxes)
        {
            try
            {
                if (port != null && port.IsOpen && !isConnectionLost)
                {
                    ushort startAddress = 0;
                    ushort numRegisters = 16; // 8 adet float veri için 16 adet register okunmalı

                    ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                    int voltage = (registers[1] << 8) | registers[0];
                    pointPairLists[0].Add(time, voltage);
                    textBoxes[0].Text = voltage.ToString();

                    float current = ConvertRegistersAkımToFloat(registers[3], registers[2]);
                    pointPairLists[1].Add(time, current);
                    textBoxes[1].Text = current.ToString();

                    float result = current * voltage;
                    int power = (int)result;
                    pointPairLists[8].Add(time, power);
                    textBoxes[8].Text = power.ToString();

                    for (int i = 2; i < 8; i++)
                    {
                        float value = ConvertRegistersToFloat(registers[i * 2 + 1], registers[i * 2]);
                        pointPairLists[i].Add(time, value);
                        textBoxes[i].Text = value.ToString();
                    }

                    time++;
                    zedGraphControl.AxisChange();
                    zedGraphControl.Invalidate();

                    // Bağlantı normal çalışıyor, bayrakları sıfırla
                    isConnectionLost = false;
                    hasShownConnectionLostMessage = false;
                }
                else if (isConnectionLost && !hasShownConnectionLostMessage)
                {
                    MessageBox.Show("Bağlantı kaybı yaşandı. Fişi geri takın.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    hasShownConnectionLostMessage = true;
                }
            }
            catch (TimeoutException)
            {
                if (!isConnectionLost)
                {
                    isConnectionLost = true;
                    hasShownConnectionLostMessage = true;
                    MessageBox.Show("Modbus zaman aşımı. Bağlantı kesilecek.", "Timeout Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SafelyStopModbus();
                    RecoverAllData();
                }
            }
            catch (IOException)
            {
                if (!isConnectionLost)
                {
                    isConnectionLost = true;
                    hasShownConnectionLostMessage = true;
                    MessageBox.Show("Seri port bağlantısı kesildi. Bağlantıyı kontrol edin.", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SafelyStopModbus();
                }
            }
            catch (Exception ex)
            {
                if (!isConnectionLost)
                {
                    isConnectionLost = true;
                    hasShownConnectionLostMessage = true;
                    MessageBox.Show($"Veri okuma sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SafelyStopModbus();
                }
            }
        }

        private void SafelyStopModbus()
        {
            try
            {
                DisconnectModbus();
                if (port != null && port.IsOpen)
                {
                    port.Close();
                }
                master = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port kapatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RecoverAllData()
        {
            try
            {
                if (pointPairLists1.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(1, pointPairLists1, time1, timer1.Interval);
                }
                if (pointPairLists2.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(2, pointPairLists2, time2, timer2.Interval);
                }
                if (pointPairLists3.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(3, pointPairLists3, time3, timer31.Interval);
                }
                if (pointPairLists4.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(4, pointPairLists4, time4, timer41.Interval);
                }
                if (pointPairLists5.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(5, pointPairLists5, time5, timer51.Interval);
                }
                if (pointPairLists6.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(6, pointPairLists6, time6, timer61.Interval);
                }
                if (pointPairLists7.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(7, pointPairLists7, time7, timer71.Interval);
                }
                if (pointPairLists8.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(8, pointPairLists8, time8, timer81.Interval);
                }
                if (pointPairLists9.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(9, pointPairLists9, time9, timer91.Interval);
                }
                if (pointPairLists10.Any(p => p.Count > 0))
                {
                    await SaveToExcelAsync(10, pointPairLists10, time10, timer101.Interval);
                }

                MessageBox.Show("Veriler başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri kurtarılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async Task SaveToExcelAsync(byte slaveId, PointPairList[] pointPairLists, int time, int interval)
        {
            try
            {
                if (pointPairLists.All(p => p.Count == 0))
                {
                    return;
                }

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Test Verileri");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (interval / 1000.0));
                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }
                    }

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string fileName = $"Test_ID_{slaveId}_{DateTime.Now:yyyy_MM_dd_HH_mm}.xlsx";
                    string filePath = Path.Combine(desktopPath, fileName);

                    FileInfo excelFile = new FileInfo(filePath);
                    await package.SaveAsAsync(excelFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }







        private float ConvertRegistersToFloat(ushort highOrderByte, ushort lowOrderByte)
        {
            // 2 adet 8-bit byte'ı 16-bit değere dönüştür
            ushort combinedValue = (ushort)((highOrderByte << 8) | lowOrderByte);

            // Dönüştürülmüş 16-bit değeri float'a dönüştür
            return combinedValue / 10.0f; // Örneğin, sıcaklık değeri 10'a bölünerek float değeri elde ediliyor
        }
        private float ConvertRegistersAkımToFloat(ushort highOrderByte, ushort lowOrderByte)
        {
            // 2 adet 8-bit byte'ı 16-bit değere dönüştür
            ushort combinedValue = (ushort)((highOrderByte << 8) | lowOrderByte);

            // Akım değerini hesapla
            float currentValue = combinedValue / 100.0f;

            // Akım kontrolü (0.40'ın altındaki değerleri 0 yap)
            if (currentValue < 0.30f)
            {
                currentValue = 0.0f;
            }

            return currentValue;
        }

        private int zaman = 1;

    /*    private async Task SaveToExcelAsync(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    // Başlık satırını ekleyin
                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            // İptal edildiğinde çıkış yap
                            return;
                        }

                        // Şu anki gerçek zamanı al
                        DateTime currentTime = DateTime.Now;
                        worksheet.Cells[i + 2, 1].Value = currentTime.ToString("HH:mm:ss");

                        // Test süresini hesapla
                        TimeSpan elapsedTime = currentTime - startTime;
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        // Veriyi ekle
                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }
                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  // Save işlemini zaman uyumsuz hale getiriyoruz.
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }  */


        private async Task SaveToExcelAsync8(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer81.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }
                    }
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer81.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private async Task SaveToExcelAsync1(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer1.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }
                    }
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer1.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private async Task SaveToExcelAsync2(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer2.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  // Save işlemini zaman uyumsuz hale getiriyoruz.
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer2.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private async Task SaveToExcelAsync3(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer31.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  // Save işlemini zaman uyumsuz hale getiriyoruz.
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer31.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private async Task SaveToExcelAsync4(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer41.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  // Save işlemini zaman uyumsuz hale getiriyoruz.
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer41.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private async Task SaveToExcelAsync5(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer51.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  // Save işlemini zaman uyumsuz hale getiriyoruz.
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer51.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private async Task SaveToExcelAsync6(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer61.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  // Save işlemini zaman uyumsuz hale getiriyoruz.
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer61.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }


        private async Task SaveToExcelAsync7(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer71.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile);  
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer71.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private async Task SaveToExcelAsync9(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer91.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }
                        
                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile); 
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer91.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private async Task SaveToExcelAsync10(byte slaveId, PointPairList[] pointPairLists, int time, CancellationToken token)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ModbusData");

                    worksheet.Cells[1, 1].Value = "Sistem Saati";
                    worksheet.Cells[1, 2].Value = "Test Süresi";
                    worksheet.Cells[1, 3].Value = "VOLTAJ";
                    worksheet.Cells[1, 4].Value = "AKIM";
                    worksheet.Cells[1, 5].Value = "NTC1";
                    worksheet.Cells[1, 6].Value = "NTC2";
                    worksheet.Cells[1, 7].Value = "NTC3";
                    worksheet.Cells[1, 8].Value = "NTC4";
                    worksheet.Cells[1, 9].Value = "NTC5";
                    worksheet.Cells[1, 10].Value = "NTC6";
                    worksheet.Cells[1, 11].Value = "WATT";

                    DateTime startTime = DateTime.Now;

                    for (int i = 0; i < time; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        DateTime simulatedCurrentTime = startTime.AddSeconds(i * (timer101.Interval / 1000.0));

                        TimeSpan elapsedTime = simulatedCurrentTime - startTime;

                        worksheet.Cells[i + 2, 1].Value = simulatedCurrentTime.ToString("HH:mm:ss");
                        worksheet.Cells[i + 2, 2].Value = elapsedTime.ToString(@"hh\:mm\:ss");

                        if (pointPairLists.Length > 0 && pointPairLists[0].Count > i)
                            worksheet.Cells[i + 2, 3].Value = pointPairLists[0][i].Y;
                        if (pointPairLists.Length > 1 && pointPairLists[1].Count > i)
                            worksheet.Cells[i + 2, 4].Value = pointPairLists[1][i].Y;
                        if (pointPairLists.Length > 2 && pointPairLists[2].Count > i)
                            worksheet.Cells[i + 2, 5].Value = pointPairLists[2][i].Y;

                        for (int j = 3; j < 9; j++)
                        {
                            if (pointPairLists.Length > j && pointPairLists[j].Count > i)
                                worksheet.Cells[i + 2, j + 3].Value = pointPairLists[j][i].Y;
                        }

                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx;*.xls",
                        Title = "Excel dosyasını kaydet"
                    };
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        await package.SaveAsAsync(excelFile); 
                        MessageBox.Show("Excel dosyası başarıyla kaydedildi.");
                    }
                    await Task.Delay(timer101.Interval, token);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }




        private void UpdateAllTimers()
        {
            if (int.TryParse(textBoxTimer1.Text, out int interval1))
            {
                timer1.Interval = interval1 * 1000; 
            }
            else
            {
                timer1.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer2.Text, out int interval2))
            {
                timer2.Interval = interval2 * 1000; 
            }
            else
            {
                timer2.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer3.Text, out int interval3))
            {
                timer31.Interval = interval3 * 1000;
            }
            else
            {
                timer31.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer4.Text, out int interval4))
            {
                timer41.Interval = interval4 * 1000;
            }
            else
            {
                timer41.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer5.Text, out int interval5))
            {
                timer51.Interval = interval5 * 1000; 
            }
            else
            {
                timer51.Interval = 1000;
            }

            if (int.TryParse(textBoxTimer6.Text, out int interval6))
            {
                timer61.Interval = interval6 * 1000; 
            }
            else
            {
                timer61.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer7.Text, out int interval7))
            {
                timer71.Interval = interval7 * 1000;
            }
            else
            {
                timer71.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer8.Text, out int interval8))
            {
                timer81.Interval = interval8 * 1000;
            }
            else
            {
                timer81.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer9.Text, out int interval9))
            {
                timer91.Interval = interval9 * 1000;
            }
            else
            {
                timer91.Interval = 1000; 
            }

            if (int.TryParse(textBoxTimer10.Text, out int interval10))
            {
                timer101.Interval = interval10 * 1000; 
            }
            else
            {
                timer101.Interval = 1000;
            }
        }

        /*   private bool HasUnsavedData()
           {
               return pointPairLists1.Any(p => p.Count > 0) || pointPairLists9.Any(p => p.Count > 0);
               // Diğer pointPairLists'i de ekleyin
           }

           private bool isClosing = false;

           private void Form1_FormClosing(object sender, FormClosingEventArgs e)
           {
               if (HasUnsavedData())
               {
                   DialogResult result = MessageBox.Show("Kaydedilmemiş veriler var. Çıkış yapmak istediğinize emin misiniz?", "Kapatma Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                   if (result == DialogResult.No)
                   {
                       e.Cancel = true;
                       return;
                   }
               }

               // Kullanıcı "Evet" dediğinde seri port bağlantısını güvenli bir şekilde kapat
               isClosing = true; // Form kapanıyor, hata mesajı göstermemek için bayrağı ayarla
               if (port != null && port.IsOpen)
               {
                   port.Close();
               }
           }  */

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectModbus();
            DialogResult result = MessageBox.Show("Veriler kaydedilmemiş olabilir. Uygulamayı kapatmak istediğinize emin misiniz?",
                                                  "Kapatma Onayı",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            if (port != null && port.IsOpen)
            {
                try
                {
                    SafelyStopModbus();
                    port.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Seri port kapatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }










        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (port != null && port.IsOpen) 
                {
                    port.Close();
                    port.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri port kapatılırken hata oluştu: {ex.Message}");
            }

            base.OnFormClosing(e);
        }
    }
}