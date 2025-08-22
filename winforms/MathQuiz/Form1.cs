namespace MathQuiz
{
    public partial class Form1 : Form
    {
        // Randomizer for generating random numbers
        Random randomizer = new Random();

        // Variables to hold the numbers for the quiz

        // for the addition problem
        int addend1;
        int addend2;

        // for the substraction problem
        int minuend;
        int subtrahend;

        // for the multiplication problem
        int multiplicand;
        int multiplier;

        // for the division problem
        int dividend;
        int divisor;

        // to keeps track of the remaining time
        int timeLeft;

        /// <summary>
        /// Start the quiz by filling in all of the problems
        /// and starging the timer
        /// </summary>
        public void StartTheQuiz()
        {
            // Generate two random numbers for the quiz
            addend1 = randomizer.Next(51);
            addend2 = randomizer.Next(51);

            // convert them into strings and display them in the text boxes
            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();

            // sum is the NumericUpDown control
            // makes sure it is set to zero
            sum.Value = 0;

            // For the subtraction problem 
            minuend = randomizer.Next(1, 101);
            subtrahend = randomizer.Next(1, minuend);
            minusLeftLabel.Text = minuend.ToString();
            minusRightLabel.Text = subtrahend.ToString();

            difference.Value = 0;

            // For the multiplication problem
            multiplicand = randomizer.Next(2, 11);
            multiplier = randomizer.Next(2, 11);
            timesLeftLabel.Text = multiplicand.ToString();
            timesRightLabel.Text = multiplier.ToString();

            product.Value = 0;

            // For the division problem
            divisor = randomizer.Next(2, 11);
            int temporaryQuotient = divisor * randomizer.Next(2, 11);
            dividend = divisor * temporaryQuotient;
            dividedLeftLabel.Text = dividend.ToString();
            dividedRightLabel.Text = divisor.ToString();

            quotient.Value = 0;

            timeLeft = 40; // set the time left to 30 seconds
            timeLabel.Text = "40 seconds"; // display the time left
            timer1.Start(); // start the timer
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartTheQuiz();
            startButton.Enabled = false;
        }

        /// <summary>
        /// Check the user's answers against the correct answers
        /// </summary>
        /// <returns></returns>
        private bool CheckTheAnswer()
        {
            if ((addend1 + addend2 == sum.Value)
                && (minuend - subtrahend == difference.Value)
                && (multiplicand * multiplier == product.Value)
                && (dividend / divisor == quotient.Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckTheAnswer())
            {
                // if CheckTheAnswer returns true, than the user
                // has answered all of the questions correctly
                // stop the timer and show a message box
                timer1.Stop();
                MessageBox.Show("Congratulations! You answered all the questions correctly!", "Congratulations");

                timeLabel.BackColor = Color.Green; // reset the color of the time label
                startButton.Enabled = true; // re-enable the start button
            }
            else if (timeLeft > 0)
            {
                // if CheckTheAnswer returns false, keep counting down.
                // Decrease the time left by one second and
                // display the new time left by update the time left label
                timeLeft = timeLeft - 1;
                timeLabel.Text = timeLeft + " seconds";

                if (timeLeft <= 10)
                {
                    timeLabel.BackColor = Color.Red; // change the color to red when time is running out
                }
            }
            else
            {
                // if timeLeft is zero, stop the timer,
                // show a message box and fill the answers
                timer1.Stop();

                timeLabel.Text = "Time's up!";
                MessageBox.Show("You didn't finish in time.", "Sorry!");

                sum.Value = addend1 + addend2;
                difference.Value = minuend - subtrahend;
                product.Value = multiplicand * multiplier;
                quotient.Value = dividend / divisor;

                timeLabel.BackColor = SystemColors.Control; // reset the color of the time label
                startButton.Enabled = true; // re-enable the start button
            }
        }

        private void answer_Enter(object sender, EventArgs e)
        {
            NumericUpDown? answerBox = sender as NumericUpDown;
            if (answerBox != null)
            {
                int lengthOfAnswer = answerBox.Value.ToString().Length;
                answerBox.Select(0, lengthOfAnswer);
            }
        }

        // notify the user when the answer is correct
        private void answer_Addition_Correctly(object sender, EventArgs e)
        {
            NumericUpDown? answer = sender as NumericUpDown;
            if (answer != null && answer.Value == (addend1 + addend2))
            {
                // make a sound that indicates is correct
                System.Media.SystemSounds.Asterisk.Play();
            }
        }
        private void answer_Subtraction_Correctly(object sender, EventArgs e)
        {
            NumericUpDown? answer = sender as NumericUpDown;
            if (answer != null && answer.Value == (minuend - subtrahend))
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
        }
        private void answer_Multiplication_Correctly(object sender, EventArgs e)
        {
            NumericUpDown? answer = sender as NumericUpDown;
            if (answer is not null && answer.Value == (multiplicand * multiplier))
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
        }
        private void answer_Division_Correctly(object sender, EventArgs e)
        {
            NumericUpDown? answer = sender as NumericUpDown;
            if (answer is not null && answer.Value == (dividend / divisor))
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
        }
    }
}
