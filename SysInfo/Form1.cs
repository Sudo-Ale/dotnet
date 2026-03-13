using Hardware.Info;
using SysInfo.Models;
using SysInfo.Presenter;
using SysInfo.Services;

namespace SysInfo
{
    public partial class Form1 : Form
    {
        private readonly HardwareInfo _hardwareInfo;
        private readonly ISizeConverter _converter;

        public Form1()
        {
            InitializeComponent();

            label1.Text = "System OS specs & Memory usage";

            _hardwareInfo = new();
            _converter = new SizeConverterServices();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _hardwareInfo.RefreshAll();
            var systemDataServices = new SystemDataServices(_hardwareInfo);
            var systemDataPresenter = new SystemDataPresenter(label2, _converter, systemDataServices);
            systemDataPresenter.ShowInfo();
        }
    }
}
