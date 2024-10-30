using BotApp.Service;

namespace BotApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BotService.GetBotChatId()
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        labelIdentifier.Text = Convert.ToString(task.Result);
                    }
                })
                .Wait();
            BotService.GetBotUsername()
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        labelUsernme.Text = Convert.ToString(task.Result);
                    }
                })
                .Wait();
        }
    }
}
