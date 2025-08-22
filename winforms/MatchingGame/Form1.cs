namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // firstClicked point to the first label control
        // that the player clicks, but it will be null initially
        Label firstClicked;

        // secondClicked point to the second label control
        // that the player clicks, but it will be null
        Label secondClicked;

        int timeInGame;


        // use this random object for randomize icons
        Random randomizer = new();

        // Each of these letters is an icon
        // in the webdings font
        // each icons appear twice in this list
        List<string> icons = new()
        {
            "!", "N", ",", "k", "b", "v", "w", "z",
            "!", "N", ",", "k", "b", "v", "w", "z"
        };

        public Form1()
        {
            InitializeComponent();

            timeInGame = 0;
            timer2.Start();

            AssignIconsToSquare();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // assign each icon from the list of icons to a random square
        private void AssignIconsToSquare()
        {
            // the TableLayoutPanel has 16 squares
            // the icon list has 16 icons
            // so an icon is pulled at random from the list
            // and assigned to each square label
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label? iconLabel = control as Label;
                if (iconLabel is not null)
                {
                    // check if there are still icons left to assign
                    int randomIndex = randomizer.Next(icons.Count);

                    // assign the icon to the label
                    iconLabel.Text = icons[randomIndex];

                    iconLabel.ForeColor = iconLabel.BackColor;

                    // remove the icon from the list
                    icons.RemoveAt(randomIndex);
                }
            }
        }

        /// <summary>
        /// Every label's Click event is handled by this method.
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            // if the timer is running, ignore the click
            if (timer1.Enabled)
            {
                return;
            }

            Label? clickedLabel = sender as Label;
            if (clickedLabel is not null)
            {
                // if the label is black, the player clicked
                // an icon that's already been revealed
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                // if firstClicked is null, this is the first icon
                // in the pair that the player clicked,
                // so set firstClicked to the label that the player clicked
                // change its color to black and return
                if (firstClicked is null)
                {
                    firstClicked = clickedLabel;

                    // reveal the icon by changing the color
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                // if the player gets here, it means that
                // firstClicked is not null, so it must be the second icon
                // that the player clicked
                // change its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // check if the player has won the game
                CheckForWinner();

                // check if the two icons match
                // if they match, reset the firstClicked and secondClicked
                // so the program can accept new clicks
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;

                    // make a sound to indicate a match
                    System.Media.SystemSounds.Beep.Play();

                    return;
                }

                // if the player gets so far,
                // it means that the player clicked two icons
                // so start the timer (wich will wait 3 quarters of a second)
                // and then hide both icons if they don't match
                timer1.Start();
            }
        }

        /// <summary>
        /// The timer is started when the player clicks
        /// two icons that don't match,
        /// so it counts three quarters of a second
        /// and then turns itself off and hides both icons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // stop the timer
            timer1.Stop();

            // hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // reset the firstClicked and secondClicked
            // so the program can accept new clicks
            // because is the first time the player clicked
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Check if every icons has been matched and revealed,
        /// If all icons are matched, the player wins the game.
        /// </summary>
        private void CheckForWinner()
        {
            // go through each control in the TableLayoutPanel
            // checking each one to see if it has been matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label? iconLabel = control as Label;

                if (iconLabel is not null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                    {
                        return;
                    }
                }
            }

            timer2.Stop(); // stop the timer2 if the player wins the game

            // if the loop didn't return,
            // it means that all icons have been matched
            // so show a message box to the player
            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timeInGame = timeInGame + 1;
            gameTimer.Text = timeInGame + " seconds";
        }
    }
}
