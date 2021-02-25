using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlockForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLockForSure(object sender, EventArgs e)
        {
            var someValue = GetVal().Result;
            (sender as Button).Text = (sender as Button).Text + someValue.ToString();
        }

        private void btNoLock_Click(object sender, EventArgs e)
        {
            var someValue = Task.Run(() => GetVal()).Result;
            (sender as Button).Text = (sender as Button).Text + someValue.ToString();
        }

        private async void btNoLockCorrect_Click(object sender, EventArgs e)
        {
            var someValue = await GetVal();
            (sender as Button).Text = (sender as Button).Text + someValue.ToString();
        }

        private async Task<int> GetVal()
        {
            await Task.Delay(10);
            return 1;
        }
    }
}
