using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CSALMongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DifferFormatShi
{
    public partial class Form1 : Form
    {
        //should change the name of the database
        public const string DB_URL = "mongodb://localhost:27017/csaldata";
        //protected MongoDatabase testDB = null;
        List<String> studentsInClass = new List<string>();

        List<String> needStudentsT = new List<string>();
        List<String> needStudentsA = new List<string>();

        // Generate all the tags
        //String Tags = "Record ID\tSubject ID\tClass ID\tQ1\tQ2\tQ3\tQ4\tQ5\tQ6\tQ7\tQ8\tQ9\tQ10\tQ11\tQ12\tQ13\tQ14\t15\t16\tQ6\tQ17\tQ18\tQ19\tQ10\tQ21\tQ12\tQ23\tQ24\tQ25\tQ26\tQ27\tQ28\tQ29\tQ30TotalTime\tQ1Time\tQ2Time\tQ3Time\tQ4Time\tQ5Time\tQ6Time\tQ7Time\tQ8Time\tQ9Time\tQ10Time\tQ11Time\tQ12Time\tLessonAttempt";
        public Form1()
        {
            InitializeComponent();


            // Start 
            try
            {
                //query from the database
                var db = new CSALDatabase(DB_URL);
                var students = db.FindStudents();
                var lessons = db.FindLessons();
                var classes = db.FindClasses();

                String Tags = "RecordID\tSubject ID\tClass ID\t" + getTag(3) + getTag(3) + getTag(3) + "\n";


                // find the target classes
                foreach (var oneClass in classes)
                {
                    //ace | kingwilliam | main | tlp
                    if (oneClass.ClassID == "pilot1_lai1" || oneClass.ClassID == "pilot1_lai2" || oneClass.ClassID == "pilot1_marietta1" || oneClass.ClassID == "pilot1_marietta2")
                    //if (oneClass.ClassID == "ace" || oneClass.ClassID == "kingwilliam" || oneClass.ClassID == "main" || oneClass.ClassID == "tlp")
                    {

                        foreach (String student in oneClass.Students)
                        {
                            if (!String.IsNullOrWhiteSpace(student) && !String.IsNullOrWhiteSpace(oneClass.ClassID))
                            {
                                String cS = oneClass.ClassID + "-" + student;
                                needStudentsA.Add(cS);
                            }

                        }
                    }
                    // lai | marietta
                    else if (oneClass.ClassID == "pilot1_ptp1" || oneClass.ClassID == "pilot1_ptp1" || oneClass.ClassID == "pilot1_aecn" || oneClass.ClassID == "pilot1_tlp")
                    //if (oneClass.ClassID == "lai" || oneClass.ClassID == "marietta")
                    {
                        foreach (String student in oneClass.Students)
                        {
                            if (!String.IsNullOrWhiteSpace(student) && !String.IsNullOrWhiteSpace(oneClass.ClassID))
                            {
                                String cS = oneClass.ClassID + "-" + student;
                                needStudentsA.Add(cS);
                            }
                        }
                    }
                }


                //this.richTextBox1.Text = needStudentsA.Count.ToString();

                List<String> allText = new List<String>();
                foreach (String student in needStudentsA)
                {
                    if (!String.IsNullOrWhiteSpace(student))
                    {
                        // need to be careful here, may not right
                        String perLesson = getInfoForOne(student);
                        if (perLesson == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (perLesson == null || perLesson == "")
                            {
                                continue;
                            }
                            else
                            {
                                allText.Add(perLesson);
                            }
                            
                        }

                        //allText += recordCount.ToString() + "\t" + getInfoForOne(studentRecord) + "\n";
                    }
                    else
                    {
                        continue;
                    }
                }

                int recordID = 0;
                this.richTextBox1.Text = Tags;
                if (allText == null || allText.Count < 1)
                {
                    this.richTextBox1.AppendText("\n ***********************Show Nothing**********************\n");
                }
                else
                {
                    foreach (String perRecord in allText)
                    {
                        recordID = recordID + 1;
                        /*
                        String record1 = perRecord.Split(new String[] { "\n" }, StringSplitOptions.None)[0];
                        String record2 = perRecord.Split(new string[] { "\n" }, StringSplitOptions.None)[1];
                        this.richTextBox1.AppendText(recordID.ToString() + "\t" + record1 + "\n" + (recordID+1).ToString() + "\t" + record2 + "\n");
                        */
                        this.richTextBox1.AppendText(recordID.ToString() + "\t" + perRecord + "\n");
                    }
                }

            }
            catch (Exception e)
            {
                e.GetBaseException();
                e.GetType();

            }

        }

        // get one student's information
        public String getInfoForOne(String studentRecord)
        {
            String allRecord = "";

            for (int i = 1; i < 35; i++)
            {
                //2 | 4 | 8 | 10 | 11 | 12 | 13 | 14 | 18 | 26 | 27
                //for Breya 
                //if (i == 1 || i == 6 || i == 12 || i == 18 || i == 21)
                // This is for Magen if(i == 4 || i == 7 || i == 9 || i == 16 || i == 30)
                // for Shi 
                //if (i == 2 || i == 13 || i == 15 || i == 19 || i == 25 || i == 28)
                // for Haiying
                if (i == 32)
                {
                    var lessonId = "lesson" + i.ToString();
                    String classID = studentRecord.Split(new Char[] { '-' })[0];
                    String subjectID = studentRecord.Split(new Char[] { '-' })[1];
                    String perRecord = getPerRecord32(subjectID, lessonId);
                    if (perRecord == null)
                    {
                        continue;
                    }
                    else
                    {
                        /*
                        String sentenceRow = perRecord.Split(new string[] { "\n" }, StringSplitOptions.None)[0];
                        String scoreRow = perRecord.Split(new string[] { "\n" }, StringSplitOptions.None)[1];
                        allRecord = subjectID + "\t" + classID + "\t" + sentenceRow + "\n" + subjectID + "\t" + classID + "\t" + scoreRow;
                    */
                        allRecord = subjectID + "\t" + classID + "\t" + perRecord;

                    }
                }
            }

            return allRecord;
        }

        // lesson 1
        public String getPerRecord1(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean firstAttemp = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // easy second trial
            List<String> questionEasySecondScore = new List<string>();
            List<String> questionEasySecondDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            // hard second trial
            List<String> questionHardSecondScore = new List<string>();
            List<String> questionHardSecondDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionSecondScore, questionSecondDura, 10)
                            + fillRecord(questionEasyScore, questionEasyDura, 10) + fillRecord(questionEasySecondScore, questionEasySecondDura, 10)
                            + fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionHardSecondScore, questionHardSecondDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySecondScore.Clear();
                        questionEasySecondDura.Clear();

                        questionHardScore.Clear();
                        questionHardDura.Clear();
                        questionHardSecondScore.Clear();
                        questionHardSecondDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                    questionEasySecondScore.Add(" ");
                                    questionEasySecondDura.Add(" ");
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                    questionHardSecondScore.Add(" ");
                                    questionHardSecondDura.Add(" ");
                                }
                            }
                            else if (transition.RuleID == "Incorrect")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID == "Correct2")
                            {
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                        }
                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionSecondScore, questionSecondDura, 10)
                            + fillRecord(questionEasyScore, questionEasyDura, 10) + fillRecord(questionEasySecondScore, questionEasySecondDura, 10)
                            + fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionHardSecondScore, questionHardSecondDura, 10);
            }
            return oneRecord;
        }

        // lesson 2
        public String getPerRecord2(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean TBIncorrect = false, reachTBAfterInc = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // TARepeat
            List<String> questionTARepeatScore = new List<string>();
            List<String> questionTARepestDura = new List<string>();

            // medium second trial
            List<String> questionl2Score = new List<string>();
            List<String> questionl2Dura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 15) 
                            + fillRecord(questionTARepeatScore, questionTARepestDura, 15)
                            + fillRecord(questionl2Score, questionl2Dura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionTARepeatScore.Clear();
                        questionTARepestDura.Clear();
                        questionScore.Clear();
                        questionDura.Clear();
                        questionl2Score.Clear();
                        questionl2Dura.Clear();

                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "TBCorrect" && transition.StateID == "ShowItemTB")
                            {
                                score = 1;
                                questionl2Score.Add(score.ToString());
                                questionl2Dura.Add(duration.ToString());
                            }
                            if (transition.RuleID == "TBIncorrect" && transition.StateID == "ShowItemTB")
                            {
                                score = 0;
                                questionl2Score.Add(score.ToString());
                                questionl2Dura.Add(duration.ToString());
                            }
                            if (transition.RuleID.ToString().Contains("ChangePageTB"))
                            {
                                if (TBIncorrect == true) {
                                    TBIncorrect = false;
                                    if (questionTARepeatScore.Count > 2)
                                    {
                                        questionTARepeatScore.RemoveRange(questionTARepeatScore.Count - 3, 2);
                                        questionTARepestDura.RemoveRange(questionTARepestDura.Count - 3, 2);
                           
                                    }
                                }
                                sectionFlag = 1;
                            }
                            if (transition.RuleID.ToString().Contains("TARepeat") && sectionFlag == 0)
                            {
                                sectionFlag = 2;
                            }
                        }

                        if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionTARepeatScore.Add(score.ToString());
                                questionTARepestDura.Add(duration.ToString());
                            }
                        }
                        else if (turn.Input.Event == "Incorrect")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionTARepeatScore.Add(score.ToString());
                                questionTARepestDura.Add(duration.ToString());
                                TBIncorrect = true;
                            }
                        }
                        
                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 15)
                            + fillRecord(questionTARepeatScore, questionTARepestDura, 15)
                            + fillRecord(questionl2Score, questionl2Dura, 10);
            }
            return oneRecord;
        }

        // lesson 3 
        public String getPerRecord3(String studentName, String lessonID)
        {
            String oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
         
            List<double> attempTime = new List<double>();
            List<double> readingTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" + fillRecord(questionScore, questionDura, 12) +
                             fillRecord(questionEasyScore, questionEasyDura, 12) + fillRecord(questionHardScore, questionHardDura, 12);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {

                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                            else if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                        }

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            // medium level, odd question number, skip
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" + fillRecord(questionScore, questionDura, 12) +
                    fillRecord(questionEasyScore, questionEasyDura, 12) + fillRecord(questionHardScore, questionHardDura, 12);
            }
            return oneRecord;
        }

        // lesson 4, 
        public String getPerRecord4(String studentName, String lessonID)
        {
            String  oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            bool getAnswer = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            // Medium 2
            List<String> questionM2Score = new List<string>();
            List<String> questionM2Dura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" +
                                fillRecord(questionScore, questionDura, 5) + fillRecord(questionEasyScore, questionEasyDura, 10) +
                                fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionM2Score, questionM2Dura, 17);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                        questionM2Score.Clear();
                        questionM2Dura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        // correctness
                        if (turn.Input.Event == "Correct" && sectionFlag == 0)
                        {
                            score = 1;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                        else if (turn.Input.Event == "Incorrect" && sectionFlag == 0)
                        {
                            score = 0;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                        else if (turn.Input.Event == "Correct" && sectionFlag == 1)
                        {
                            score = 1;
                            questionEasyScore.Add(score.ToString());
                            questionEasyDura.Add(duration.ToString());
                        }
                        else if ((turn.Input.Event == "Incorrect") && sectionFlag == 1)
                        {
                            score = 0;
                            questionEasyScore.Add(score.ToString());
                            questionEasyDura.Add(duration.ToString());
                        }
                        else if (turn.Input.Event == "Correct" && sectionFlag == 2)
                        {
                            score = 1;
                            questionHardScore.Add(score.ToString());
                            questionHardDura.Add(duration.ToString());
                        }
                        else if ((turn.Input.Event == "Incorrect") && sectionFlag == 2 )
                        {
                            score = 0;
                            questionHardScore.Add(score.ToString());
                            questionHardDura.Add(duration.ToString());
                        }
                        else if (turn.Input.Event == "Correct" && sectionFlag == 3 )
                        {
                            score = 1;
                            questionM2Score.Add(score.ToString());
                            questionM2Dura.Add(duration.ToString());
                        }
                        else if ((turn.Input.Event == "Incorrect") && sectionFlag == 3 )
                        {
                            score = 0;
                            questionM2Score.Add(score.ToString());
                            questionM2Dura.Add(duration.ToString());
                        }

                        foreach (var transition in turn.Transitions)
                        {
                            // section level
                            if (transition.RuleID == "GetTutoringPackEasy" && sectionFlag == 0)
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard" && sectionFlag == 0)
                            {
                                sectionFlag = 2;
                            }
                            if (turn.Input.Event.ToString().Contains("Level2_Diagnostic"))
                            {
                                sectionFlag = 3;
                            }

                            
                        }
                    }

                    lastTurnID = turn.TurnID;
                }

              
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" +
                        fillRecord(questionScore, questionDura, 5) + fillRecord(questionEasyScore, questionEasyDura, 10) +
                        fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionM2Score, questionM2Dura, 17);
            }
            return oneRecord;
        }

        // lesson 5 
        public String getPerRecord5(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            String questionRow = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            bool getAnswer = false, firstSectionRead = true;

            List<double> attempTime = new List<double>();
            List<double> readingTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();
            
            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();
            
            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();
            
            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                double startReadFirst = 0, startReadSecond = 0;
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" + fillRecord(questionScore, questionDura, 10) +
                             fillRecord(questionEasyScore, questionEasyDura, 8) + fillRecord(questionHardScore, questionHardDura, 12);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {

                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                            else if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                        }

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            getAnswer = false;
                            score = 0;
                            // medium level, odd question number, skip
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            getAnswer = false;
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" + fillRecord(questionScore, questionDura, 10) +
                    fillRecord(questionEasyScore, questionEasyDura, 8) + fillRecord(questionHardScore, questionHardDura, 12);
            }
            return oneRecord;
        }

        // lesson 6
        public String getPerRecord6(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean firstAttemp = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // easy second trial
            List<String> questionEasySecondScore = new List<string>();
            List<String> questionEasySecondDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            // hard second trial
            List<String> questionHardSecondScore = new List<string>();
            List<String> questionHardSecondDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {

                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionSecondScore, questionSecondDura, 10)
                            + fillRecord(questionEasyScore, questionEasyDura, 10) + fillRecord(questionEasySecondScore, questionEasySecondDura, 10)
                            + fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionHardSecondScore, questionHardSecondDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySecondScore.Clear();
                        questionEasySecondDura.Clear();

                        questionHardScore.Clear();
                        questionHardDura.Clear();
                        questionHardSecondScore.Clear();
                        questionHardSecondDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                    questionEasySecondScore.Add(" ");
                                    questionEasySecondDura.Add(" ");
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                    questionHardSecondScore.Add(" ");
                                    questionHardSecondDura.Add(" ");
                                }
                            }
                            else if (transition.RuleID == "Incorrect")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID == "Correct2")
                            {
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                        }
                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionSecondScore, questionSecondDura, 10)
                            + fillRecord(questionEasyScore, questionEasyDura, 10) + fillRecord(questionEasySecondScore, questionEasySecondDura, 10)
                            + fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionHardSecondScore, questionHardSecondDura, 10);
            }
            return oneRecord;
        }


        // lesson 7, 
        public String getPerRecord7(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 12) + fillRecord(questionEasyScore, questionEasyDura, 8) + fillRecord(questionHardScore, questionHardDura, 8);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }


                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event == "Incorrect" || turn.Input.Event == "Incorrect1")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }

                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 12) + fillRecord(questionEasyScore, questionEasyDura, 8) + fillRecord(questionHardScore, questionHardDura, 8);

            }
            return oneRecord;
        }


        // lesson 8
        public String getPerRecord8(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean firstAttemp = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();


            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {

                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var trans in turn.Transitions)
                        {
                            //Analyze actions to see if they add user score
                            foreach (var action in trans.Actions)
                            {

                                if (action.Act == "AddUserScore" && action.Data == "1")
                                {
                                    score = 1;
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    break;
                                }
                                else if (action.Act == "GetMediaFeedback" && action.Data == "SAGoodAnswer")
                                {
                                    score = 0;
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    break;
                                }
                            }
                        } 

                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10);
            }
            return oneRecord;
        }


        // lesson 9, 
        public String getPerRecord9(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11) + fillRecord(questionEasyScore, questionEasyDura, 12) + fillRecord(questionHardScore, questionHardDura, 12);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }


                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event == "Incorrect" || turn.Input.Event == "Incorrect1")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }

                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11) + fillRecord(questionEasyScore, questionEasyDura, 12) + fillRecord(questionHardScore, questionHardDura, 12);

            }
            return oneRecord;
        }


        // lesson 10
        public String getPerRecord10(String studentName, String lessonID)
        {
            String oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            
            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {

                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 36) + fillRecord(questionSecondScore, questionSecondDura, 36);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                            }
                            else if (transition.RuleID == "Incorrect")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }

                            }
                            else if (transition.RuleID == "Correct2")
                            {
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                            }

                        }
                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 36) + fillRecord(questionSecondScore, questionSecondDura, 36);
            }
            return oneRecord;
        }

        // lesson 11, 
        public String getPerRecord11(String studentName, String lessonID)
        {
            String oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, questionIndex = 0, attempt = 0;
            Boolean reachAskQ = false;
            double score = 0, duration = 0, secondDura = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // initial all the list with 16 items
            for (int i = 0; i < 16; i++)
            {
                questionScore.Add(' '.ToString());
                questionDura.Add(' '.ToString());
                questionSecondScore.Add(' '.ToString());
                questionSecondDura.Add(' '.ToString());
            }

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 16) + fillRecord(questionSecondScore, questionSecondDura, 16);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        for (int i = 0; i < 16; i++)
                        {
                            questionScore.Add(' '.ToString());
                            questionDura.Add(' '.ToString());
                            questionSecondScore.Add(' '.ToString());
                            questionSecondDura.Add(' '.ToString());
                        }

                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID.Contains("AskQ"))
                            {
                                reachAskQ = true;
                                int index = transition.RuleID.IndexOf("AskQ");
                                string cleanQues = (index < 0)
                                    ? transition.RuleID
                                    : transition.RuleID.Remove(index, "AskQ".Length);

                                questionIndex = Int32.Parse(cleanQues.Split(new Char[] { '.' })[0]);
                                attempt = Int32.Parse(cleanQues.Split(new Char[] { '.' })[1]);
                                break;
                            }
                        }
                    }

                    if (turn.Input.Event.Contains("Correct") && reachAskQ == true)
                    {
                        reachAskQ = false;
                        if (attempt == 1)
                        {
                            questionScore[questionIndex - 1] = '1'.ToString();
                            questionDura[questionIndex - 1] = duration.ToString();
                        }
                        else if (attempt == 2)
                        {
                            questionSecondScore[questionIndex - 1] = "0.5".ToString();
                            questionSecondDura[questionIndex - 1] = duration.ToString();
                        }
                    }
                    else if (turn.Input.Event.Contains("Incorrect") && reachAskQ == true)
                    {
                        reachAskQ = false;
                        if (attempt == 1)
                        {
                            questionScore[questionIndex - 1] = '0'.ToString();
                            questionDura[questionIndex - 1] = duration.ToString();
                        }
                        else if (attempt == 2)
                        {
                            questionSecondScore[questionIndex - 1] = "0".ToString();
                            questionSecondDura[questionIndex - 1] = duration.ToString();
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                             + fillRecord(questionScore, questionDura, 16) + fillRecord(questionSecondScore, questionSecondDura, 16);
            }
            return oneRecord;
        }

        // lesson 12
        public String getPerRecord12(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean firstAttemp = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {

                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11) + fillRecord(questionSecondScore, questionSecondDura, 11);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                            }
                            else if (transition.RuleID == "Incorrect")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                
                            }
                            else if (transition.RuleID == "Correct2")
                            {
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                            }
                            
                        }
                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11) + fillRecord(questionSecondScore, questionSecondDura, 11);
            }
            return oneRecord;
        }

        // lesson 13
        public String getPerRecord13(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 6);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 6);
            }
            return oneRecord;
        }

        // lesson 14
        public String getPerRecord14(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 17);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 17);
            }
            return oneRecord;
        }

        // lesson 15, 
        public String getPerRecord15(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            String questionRow = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            bool getAnswer = false, firstSectionRead = true;

            List<double> attempTime = new List<double>();
            List<double> readingTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();
            List<String> questionSentence = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();
            List<String> questionEasySentence = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();
            List<String> questionHardSentence = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                double startReadFirst = 0, startReadSecond = 0;
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {   
                            oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" +
                                startReadFirst + "\t" + fillRecord(questionScore, questionDura, 11) +
                                startReadSecond + "\t" + fillRecord(questionEasyScore, questionEasyDura, 11) +
                                startReadSecond + "\t" + fillRecord(questionHardScore, questionHardDura, 11);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        startReadFirst = 0;
                        startReadSecond = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionSentence.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySentence.Clear();
                        questionHardSentence.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        // compute reading time
                        if (turn.Input.Event == "SecondReadingStop")
                        {
                            double turnDura = (int)turn.Duration;
                            if (firstSectionRead)
                            {
                                startReadFirst += turnDura / 1000;
                                firstSectionRead = false;
                            }
                            else
                            {
                                startReadSecond = turnDura / 1000;
                                firstSectionRead = false;
                            }
                        }

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.StateID == "Read")
                            {
                                double turnDura = (int)turn.Duration;
                                if (firstSectionRead)
                                {
                                    startReadFirst = turnDura / 1000;
                                }
                                else
                                {
                                    startReadSecond = turnDura / 1000;
                                }
                                
                            }
                        }

                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackHard"){
                                sectionFlag = 2;
                            }
                            else if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }

                            // get the question
                            if (transition.RuleID == "HasItem")
                            {
                                foreach (var action in transition.Actions)
                                {
                                    if (action.Agent == "System" && action.Act == "Display")
                                    {
                                        if (sectionFlag == 0)
                                        {
                                            getAnswer = true;
                                            questionSentence.Add(action.Data);
                                        }
                                        else if (sectionFlag == 1)
                                        {
                                            getAnswer = true;
                                            questionEasySentence.Add(action.Data);
                                        }
                                        else if (sectionFlag == 2)
                                        {
                                            getAnswer = true;
                                            questionHardSentence.Add(action.Data);
                                        }
                                    }
                                }
                            }
                        }

                        if (turn.Input.Event.ToString().Contains("Incorrect") && getAnswer)
                        {
                            getAnswer = false;
                            score = 0;
                            // medium level, odd question number, skip
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct" && getAnswer)
                        {
                            // score & attempt
                            getAnswer = false;
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" +
                    startReadFirst + "\t" + fillRecord(questionScore, questionDura, 11) +
                    startReadSecond + "\t" + fillRecord(questionEasyScore, questionEasyDura, 11) +
                    startReadSecond + "\t" + fillRecord(questionHardScore, questionHardDura, 11);
            }

            return oneRecord;
        }

        // lesson 16, 
        public String getPerRecord16(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11) + fillRecord(questionEasyScore, questionEasyDura, 10) + fillRecord(questionHardScore, questionHardDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }


                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event == "Incorrect" || turn.Input.Event == "Incorrect1")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }

                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11) + fillRecord(questionEasyScore, questionEasyDura, 10) + fillRecord(questionHardScore, questionHardDura, 10);

            }
            return oneRecord;
        }

        // lesson 17, 
        public String getPerRecord17(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9) + fillRecord(questionEasyScore, questionEasyDura, 9) + fillRecord(questionHardScore, questionHardDura, 9);

                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct2" || transition.RuleID == "Correct1" || transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID == "Incorrect" || transition.RuleID == "Incorrect1" || transition.RuleID == "Incorrect2")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }


                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                        }
                    }
                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9) + fillRecord(questionEasyScore, questionEasyDura, 9) + fillRecord(questionHardScore, questionHardDura, 9);

            }
            return oneRecord;
        }

        // lesson 18, 
        public String getPerRecord18(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, questionIndex = 0, attempt = 0;
            Boolean reachAskQ = false;
            double score = 0, duration = 0, secondDura = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // initial all the list with 16 items
            for (int i = 0; i < 16; i++)
            {
                questionScore.Add(' '.ToString());
                questionDura.Add(' '.ToString());
                questionSecondScore.Add(' '.ToString());
                questionSecondDura.Add(' '.ToString());
            }

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 16) + fillRecord(questionSecondScore, questionSecondDura, 16);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        for (int i = 0; i < 16; i++)
                        {
                            questionScore.Add(' '.ToString());
                            questionDura.Add(' '.ToString());
                            questionSecondScore.Add(' '.ToString());
                            questionSecondDura.Add(' '.ToString());
                        }

                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID.Contains("AskQ"))
                            {
                                reachAskQ = true;
                                int index = transition.RuleID.IndexOf("AskQ");
                                string cleanQues = (index < 0)
                                    ? transition.RuleID
                                    : transition.RuleID.Remove(index, "AskQ".Length);

                                questionIndex = Int32.Parse(cleanQues.Split(new Char[] { '.' })[0]);
                                attempt = Int32.Parse(cleanQues.Split(new Char[] { '.' })[1]);
                                break;
                            }
                        }
                    }

                    if (turn.Input.Event.Contains("Correct") && reachAskQ == true)
                    {
                        reachAskQ = false;
                        if (attempt == 1)
                        {
                            questionScore[questionIndex - 1] = '1'.ToString();
                            questionDura[questionIndex - 1] = duration.ToString();
                        }
                        else if (attempt == 2)
                        {
                            questionSecondScore[questionIndex - 1] = "0.5".ToString();
                            questionSecondDura[questionIndex - 1] = duration.ToString();
                        }
                    }
                    else if (turn.Input.Event.Contains("Incorrect") && reachAskQ == true)
                    {
                        reachAskQ = false;
                        if (attempt == 1)
                        {
                            questionScore[questionIndex - 1] = '0'.ToString();
                            questionDura[questionIndex - 1] = duration.ToString();
                        }
                        else if (attempt == 2)
                        {
                            questionSecondScore[questionIndex - 1] = "0".ToString();
                            questionSecondDura[questionIndex - 1] = duration.ToString();
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                             + fillRecord(questionScore, questionDura, 16) + fillRecord(questionSecondScore, questionSecondDura, 16);
            }
            return oneRecord;
        }

        // lesson 19, 
        public String getPerRecord19(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9) + fillRecord(questionEasyScore, questionEasyDura, 6) + fillRecord(questionHardScore, questionHardDura, 8);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9) + fillRecord(questionEasyScore, questionEasyDura, 6) + fillRecord(questionHardScore, questionHardDura, 8);

            }
            return oneRecord;
        }


        // lesson 20
        public String getPerRecord20(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean TBIncorrect = false, reachTBAfterInc = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionl2Score = new List<string>();
            List<String> questionl2Dura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11)
                            + fillRecord(questionl2Score, questionl2Dura, 9);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionl2Score.Clear();
                        questionl2Dura.Clear();

                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID.ToString().Contains("ChangeLevelHard"))
                            {
                                sectionFlag = 2;
                            }
                        }

                        if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionl2Score.Add(score.ToString());
                                questionl2Dura.Add(duration.ToString());
                            }
                        }
                        else if (turn.Input.Event == "Incorrect")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionl2Score.Add(score.ToString());
                                questionl2Dura.Add(duration.ToString());
                            }
                        }

                        lastTurnID = turn.TurnID;
                    }
                }

                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 11)
                            + fillRecord(questionl2Score, questionl2Dura, 9);
            }
            return oneRecord;
        }


        // lesson 21, 
        public String getPerRecord21(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 12) + fillRecord(questionEasyScore, questionEasyDura, 12) + fillRecord(questionHardScore, questionHardDura, 12);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID == "Incorrect")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }

                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 12) + fillRecord(questionEasyScore, questionEasyDura, 12) + fillRecord(questionHardScore, questionHardDura, 12);

            }
            return oneRecord;
        }

        // lesson 22
        public String getPerRecord22(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0, secondDura = 0;
            bool skip = true, firstSectionRead = true;

            List<double> attempTime = new List<double>();
            List<double> readingTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // easy second trial
            List<String> questionEasySecondScore = new List<string>();
            List<String> questionEasySecondDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();
            
            // hard second trial
            List<String> questionHardSecondScore = new List<string>();
            List<String> questionHardSecondDura = new List<string>();


            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" 
                            + fillRecord(questionScore, questionDura, 5) + fillRecord(questionSecondScore, questionSecondDura, 5)
                            + fillRecord(questionEasyScore, questionEasyDura, 6) + fillRecord(questionEasySecondScore, questionEasySecondDura, 6)
                            + fillRecord(questionHardScore, questionHardDura, 6) + fillRecord(questionHardSecondScore, questionHardSecondDura, 6);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        secondDura = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySecondScore.Clear();
                        questionEasySecondDura.Clear();

                        questionHardScore.Clear();
                        questionHardDura.Clear();
                        questionHardSecondScore.Clear();
                        questionHardSecondDura.Clear();
                    }
                    else
                    {

                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "TPDifficult")
                            {
                                sectionFlag = 2;
                            }
                            else if (transition.RuleID == "TPEasy")
                            {
                                sectionFlag = 1;
                            }

                            // first attempt correct
                            if (transition.RuleID == "CorrectJordanCor" || transition.RuleID == "CorrectJordanInor" || transition.RuleID == "AnyQCorrectJordanCor" ||
                                transition.RuleID == "AnyQCorrectJordanInor")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                    questionEasySecondScore.Add(" ");
                                    questionEasySecondDura.Add(" ");
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                    questionHardSecondScore.Add(" ");
                                    questionHardSecondDura.Add(" ");
                                }
                            }
                            // first attempt incorrect
                            else if (transition.RuleID == "UserAnswer" || transition.RuleID == "AnyQUserAnswer")
                            {
                                secondDura += duration;
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(secondDura.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(secondDura.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(secondDura.ToString());
                                }
                                secondDura = 0;
                            }

                            // second trial correct
                            if (transition.RuleID == "HintCorrect")
                            {
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID == "HintIncorrect")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 5) + fillRecord(questionSecondScore, questionSecondDura, 5)
                            + fillRecord(questionEasyScore, questionEasyDura, 6) + fillRecord(questionEasySecondScore, questionEasySecondDura, 6)
                            + fillRecord(questionHardScore, questionHardDura, 6) + fillRecord(questionHardSecondScore, questionHardSecondDura, 6);
            }
            return oneRecord;
        }

        // lesson 23, 
        public String getPerRecord23(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            Boolean reachAskQ = false, reachAnyQ = false;
            double score = 0, duration = 0, secondDura = 0 ;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // easy second trial
            List<String> questionEasySecondScore = new List<string>();
            List<String> questionEasySecondDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            // hard second trial
            List<String> questionHardSecondScore = new List<string>();
            List<String> questionHardSecondDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9) + fillRecord(questionSecondScore, questionSecondDura, 9)
                            + fillRecord(questionEasyScore, questionEasyDura, 9) + fillRecord(questionEasySecondScore, questionEasySecondDura, 9)
                            + fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionHardSecondScore, questionHardSecondDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySecondScore.Clear();
                        questionEasySecondDura.Clear();

                        questionHardScore.Clear();
                        questionHardDura.Clear();
                        questionHardSecondScore.Clear();
                        questionHardSecondDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (turn.UserID == "Toronto-KINGWILLIAM-HSC102101413" && turn.TurnID == 92)
                            {
                                String scorefor = "Show";
                            }
                            if (transition.StateID == "AskQ" && transition.RuleID == "UserAnswer")
                            {
                                secondDura += duration;
                                reachAskQ = true;
                                continue;
                            }
                            else if (transition.StateID == "AnyQ" || transition.StateID == "HintQ")
                            {
                                reachAnyQ = true;
                            }
                            // last question of Hard
                            if (transition.RuleID == "IncorrectSummary" && transition.StateID == "AskQ")
                            {
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add("0".ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add("0".ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add("0".ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            // last question of Hard
                            else if (transition.RuleID == "CorrectSummary" && transition.StateID == "AskQ")
                            {
                                reachAskQ = false;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add("1".ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add("1".ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add("1".ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            // first attempt correct
                            else if (transition.RuleID.Contains("Correct") && transition.StateID == "AskQ")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                    questionEasySecondScore.Add(" ");
                                    questionEasySecondDura.Add(" ");
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                    questionHardSecondScore.Add(" ");
                                    questionHardSecondDura.Add(" ");
                                }
                            }
                            // first attempt incorrect
                            else if (transition.RuleID.Contains("Incorrect") && reachAskQ == true)
                            {
                                secondDura += duration;
                                reachAskQ = false;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(turn.Input.Event);
                                    questionDura.Add(secondDura.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(turn.Input.Event);
                                    questionEasyDura.Add(secondDura.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(turn.Input.Event);
                                    questionHardDura.Add(secondDura.ToString());
                                }
                                secondDura = 0;
                            }
                            
                            // second trial correct
                            else if (transition.RuleID.Contains("Correct") && reachAnyQ == true)
                            {
                                reachAnyQ = false;
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID.Contains("Incorrect") && reachAnyQ == true)
                            {
                                reachAnyQ = false;
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }


                            if (transition.RuleID == "MediaLoadedEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "MediaLoadedHard" || transition.RuleID == "MediaLoadedDifficult")
                            {
                                sectionFlag = 2;
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                             + fillRecord(questionScore, questionDura, 9) + fillRecord(questionSecondScore, questionSecondDura, 9)
                            + fillRecord(questionEasyScore, questionEasyDura, 9) + fillRecord(questionEasySecondScore, questionEasySecondDura, 9)
                            + fillRecord(questionHardScore, questionHardDura, 10) + fillRecord(questionHardSecondScore, questionHardSecondDura, 10);
            }
            return oneRecord;
        }

        // lesson 24, 
        public String getPerRecord24(String studentName, String lessonID)
        {
            String oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0, secondDura = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // easy second trial
            List<String> questionEasySecondScore = new List<string>();
            List<String> questionEasySecondDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            // hard second trial
            List<String> questionHardSecondScore = new List<string>();
            List<String> questionHardSecondDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9) + fillRecord(questionSecondScore, questionSecondDura, 9)
                            + fillRecord(questionEasyScore, questionEasyDura, 14) + fillRecord(questionEasySecondScore, questionEasySecondDura, 14)
                            + fillRecord(questionHardScore, questionHardDura, 9) + fillRecord(questionHardSecondScore, questionHardSecondDura, 9);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySecondScore.Clear();
                        questionEasySecondDura.Clear();

                        questionHardScore.Clear();
                        questionHardDura.Clear();
                        questionHardSecondScore.Clear();
                        questionHardSecondDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                    questionSecondScore.Add(" ");
                                    questionSecondDura.Add(" ");
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                    questionEasySecondScore.Add(" ");
                                    questionEasySecondDura.Add(" ");
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                    questionHardSecondScore.Add(" ");
                                    questionHardSecondDura.Add(" ");
                                }
                            }
                            // first attempt incorrect
                            else if (transition.RuleID == "Incorrect")
                            {
                                secondDura += duration;
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(secondDura.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(secondDura.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(secondDura.ToString());
                                }
                                secondDura = 0;
                            }

                            // second trial correct
                            else if (transition.RuleID == "Correct2")
                            {
                                score = 0.5;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }
                            else if (transition.RuleID == "Incorrect2")
                            {
                                score = 0;
                                if (sectionFlag == 0)
                                {
                                    questionSecondScore.Add(score.ToString());
                                    questionSecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasySecondScore.Add(score.ToString());
                                    questionEasySecondDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardSecondScore.Add(score.ToString());
                                    questionHardSecondDura.Add(duration.ToString());
                                }
                            }


                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                             + fillRecord(questionScore, questionDura, 9) + fillRecord(questionSecondScore, questionSecondDura, 9)
                            + fillRecord(questionEasyScore, questionEasyDura, 14) + fillRecord(questionEasySecondScore, questionEasySecondDura, 14)
                            + fillRecord(questionHardScore, questionHardDura, 9) + fillRecord(questionHardSecondScore, questionHardSecondDura, 9);
            }
            return oneRecord;
        }

        // lesson 25, 
        public String getPerRecord25(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            Boolean firstAttemp = false;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionEasyScore, questionEasyDura, 11) + fillRecord(questionHardScore, questionHardDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetEasyTutoringPack")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetHardTutoringPack")
                            {
                                sectionFlag = 2;
                            }
                            if (transition.RuleID == "GetMediumTutoringPack")
                            {
                                sectionFlag = 3;
                            }


                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event == "Incorrect" || turn.Input.Event == "Incorrect1")
                        {

                            if (turn.Input.Event == "Incorrect1")
                            {
                                firstAttemp = true;
                            }
                            score = 0;
                            if (sectionFlag == 3)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            if (firstAttemp == false)
                            {
                                // score & attempt
                                score = 1;
                                if (sectionFlag == 3)
                                {
                                    questionScore.Add(score.ToString());
                                    questionDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 1)
                                {
                                    questionEasyScore.Add(score.ToString());
                                    questionEasyDura.Add(duration.ToString());
                                }
                                else if (sectionFlag == 2)
                                {
                                    questionHardScore.Add(score.ToString());
                                    questionHardDura.Add(duration.ToString());
                                }
                            }
                            else
                            {
                                firstAttemp = false;
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionEasyScore, questionEasyDura, 11) + fillRecord(questionHardScore, questionHardDura, 10);

            }
            return oneRecord;
        }

        // lesson 26, 
        public String getPerRecord26(String studentName, String lessonID)
        {
            String oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, questionIndex = 0, attempt = 0;
            Boolean reachAskQ = false;
            double score = 0, duration = 0, secondDura = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // medium second trial
            List<String> questionSecondScore = new List<string>();
            List<String> questionSecondDura = new List<string>();

            // initial all the list with 16 items
            for (int i = 0; i < 16; i++)
            {
                questionScore.Add(' '.ToString());
                questionDura.Add(' '.ToString());
                questionSecondScore.Add(' '.ToString());
                questionSecondDura.Add(' '.ToString());
            }

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 16) + fillRecord(questionSecondScore, questionSecondDura, 16);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;

                        questionScore.Clear();
                        questionDura.Clear();
                        questionSecondScore.Clear();
                        questionSecondDura.Clear();

                        for (int i = 0; i < 16; i++)
                        {
                            questionScore.Add(' '.ToString());
                            questionDura.Add(' '.ToString());
                            questionSecondScore.Add(' '.ToString());
                            questionSecondDura.Add(' '.ToString());
                        }

                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID.Contains("AskQ"))
                            {
                                reachAskQ = true;
                                int index = transition.RuleID.IndexOf("AskQ");
                                string cleanQues = (index < 0)
                                    ? transition.RuleID
                                    : transition.RuleID.Remove(index, "AskQ".Length);

                                questionIndex = Int32.Parse(cleanQues.Split(new Char[] { '.' })[0]);
                                attempt = Int32.Parse(cleanQues.Split(new Char[] { '.' })[1]);
                                break;
                            }
                        }
                    }

                    if (turn.Input.Event.Contains("Correct") && reachAskQ == true)
                    {
                        reachAskQ = false;
                        if (attempt == 1)
                        {
                            questionScore[questionIndex - 1] = '1'.ToString();
                            questionDura[questionIndex - 1] = duration.ToString();
                        }
                        else if (attempt == 2)
                        {
                            questionSecondScore[questionIndex - 1] = "0.5".ToString();
                            questionSecondDura[questionIndex - 1] = duration.ToString();
                        }
                    }
                    else if (turn.Input.Event.Contains("Incorrect") && reachAskQ == true)
                    {
                        reachAskQ = false;
                        if (attempt == 1)
                        {
                            questionScore[questionIndex - 1] = '0'.ToString();
                            questionDura[questionIndex - 1] = duration.ToString();
                        }
                        else if (attempt == 2)
                        {
                            questionSecondScore[questionIndex - 1] = "0".ToString();
                            questionSecondDura[questionIndex - 1] = duration.ToString();
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                             + fillRecord(questionScore, questionDura, 16) + fillRecord(questionSecondScore, questionSecondDura, 16);
            }
            return oneRecord;
        }

        // lesson 27
        public String getPerRecord27(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 9);
            }
            return oneRecord;
        }

        // lesson 28, 
        public String getPerRecord28(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            String questionRow = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;
            bool getAnswer = false, firstSectionRead = true;

            List<double> attempTime = new List<double>();
            List<double> readingTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();
            List<String> questionSentence = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();
            List<String> questionEasySentence = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();
            List<String> questionHardSentence = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                double startReadFirst = 0, startReadSecond = 0;
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" +
                            startReadFirst + "\t" + fillRecord(questionScore, questionDura, 10) +
                            startReadSecond + "\t" + fillRecord(questionEasyScore, questionEasyDura, 10) +
                            startReadSecond + "\t" + fillRecord(questionHardScore, questionHardDura, 10);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        startReadFirst = 0;
                        startReadSecond = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionSentence.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionEasySentence.Clear();
                        questionHardSentence.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        // compute reading time
                        if (turn.Input.Event == "SecondReadingStop")
                        {
                            double turnDura = (int)turn.Duration;
                            if (firstSectionRead)
                            {
                                startReadFirst += turnDura / 1000;
                                firstSectionRead = false;
                            }
                            else
                            {
                                startReadSecond = turnDura / 1000;
                                firstSectionRead = false;
                            }
                        }

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.StateID == "Read")
                            {
                                double turnDura = (int)turn.Duration;
                                if (firstSectionRead)
                                {
                                    startReadFirst = turnDura / 1000;
                                }
                                else
                                {
                                    startReadSecond = turnDura / 1000;
                                }

                            }
                        }

                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }
                            else if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }

                            // get the question
                            if (transition.RuleID == "HasItem")
                            {
                                foreach (var action in transition.Actions)
                                {
                                    if (action.Agent == "System" && action.Act == "Display")
                                    {
                                        if (sectionFlag == 0)
                                        {
                                            getAnswer = true;
                                            questionSentence.Add(action.Data);
                                        }
                                        else if (sectionFlag == 1)
                                        {
                                            getAnswer = true;
                                            questionEasySentence.Add(action.Data);
                                        }
                                        else if (sectionFlag == 2)
                                        {
                                            getAnswer = true;
                                            questionHardSentence.Add(action.Data);
                                        }
                                    }
                                }
                            }
                        }

                        if (turn.Input.Event.ToString().Contains("Incorrect") && getAnswer)
                        {
                            getAnswer = false;
                            score = 0;
                            // medium level, odd question number, skip
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct" && getAnswer)
                        {
                            // score & attempt
                            getAnswer = false;
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t" +
                    startReadFirst + "\t" + fillRecord(questionScore, questionDura, 10) +
                    startReadSecond + "\t" + fillRecord(questionEasyScore, questionEasyDura, 10) +
                    startReadSecond + "\t" + fillRecord(questionHardScore, questionHardDura, 10);
            }

            return oneRecord;
        }

        // lesson 29, 
        public String getPerRecord29(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 16) + fillRecord(questionEasyScore, questionEasyDura, 15) + fillRecord(questionHardScore, questionHardDura, 16);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }


                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event == "Incorrect" || turn.Input.Event == "Incorrect1")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                            
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 16) + fillRecord(questionEasyScore, questionEasyDura, 15) + fillRecord(questionHardScore, questionHardDura, 16);

            }
            return oneRecord;
        }


        // lesson 30, 
        public String getPerRecord30(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0, sectionFlag = 0;
            double score = 0, duration = 0;

            List<double> attempTime = new List<double>();

            // medium
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            // easy
            List<String> questionEasyScore = new List<string>();
            List<String> questionEasyDura = new List<string>();

            // hard
            List<String> questionHardScore = new List<string>();
            List<String> questionHardDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionEasyScore, questionEasyDura, 8) + fillRecord(questionHardScore, questionHardDura, 11);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        sectionFlag = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                        questionEasyScore.Clear();
                        questionEasyDura.Clear();
                        questionHardScore.Clear();
                        questionHardDura.Clear();
                    }
                    else
                    {
                        foreach (var transition in turn.Transitions)
                        {
                            if (transition.RuleID == "GetTutoringPackEasy")
                            {
                                sectionFlag = 1;
                            }
                            if (transition.RuleID == "GetTutoringPackHard")
                            {
                                sectionFlag = 2;
                            }


                        }
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event == "Incorrect" || turn.Input.Event == "Incorrect1")
                        {
                            score = 0;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            if (sectionFlag == 0)
                            {
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 1)
                            {
                                questionEasyScore.Add(score.ToString());
                                questionEasyDura.Add(duration.ToString());
                            }
                            else if (sectionFlag == 2)
                            {
                                questionHardScore.Add(score.ToString());
                                questionHardDura.Add(duration.ToString());
                            }

                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 10) + fillRecord(questionEasyScore, questionEasyDura, 8) + fillRecord(questionHardScore, questionHardDura, 11);

            }
            return oneRecord;
        }

        // lesson 31
        public String getPerRecord31(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            bool getTpa4 = false, getTpa8 = false;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 8);
                        thisAttempCount++;
                        score = 0;
                        getTpa4 = false;
                        getTpa8 = false;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }

                        foreach (var ass in turn.Assessments)
                        {
                            if (ass.TargetID == "TutoringPack TPA4" && getTpa4 == false)
                            {
                                getTpa4 = true;
                                questionScore.Add(turn.Input.CurrentText);
                                questionDura.Add(duration.ToString());
                            }
                            if (ass.TargetID == "TutoringPack TPA8" && getTpa8 == false)
                            {
                                getTpa8 = true;
                                questionScore.Add(turn.Input.CurrentText);
                                questionDura.Add(duration.ToString());
                            }
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 8);
            }
            return oneRecord;
        }

        // lesson 32
        public String getPerRecord32(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            bool getTpa4 = false, getTpa8 = false;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 19);
                        thisAttempCount++;
                        score = 0;
                        getTpa4 = false;
                        getTpa8 = false;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;
                        
                        if (turn.Input.Event == "Incorrect")
                            {
                                score = 0;
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (turn.Input.Event == "Correct")
                            {
                                // score & attempt
                                score = 1;
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }

                        if (turn.Input.CurrentText != "")
                        {
                            questionScore.Add(turn.Input.CurrentText.ToString());
                            questionDura.Add(duration.ToString());
                            
                        }
                        
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 19);
            }
            return oneRecord;
        }

        // lesson 33
        public String getPerRecord33(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            bool getTpa4 = false, getTpa8 = false;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 13);
                        thisAttempCount++;
                        score = 0;
                        getTpa4 = false;
                        getTpa8 = false;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        foreach (var trans in turn.Transitions)
                        {
                            if (trans.RuleID.Contains("Incorrect"))
                            {
                                score = 0;
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                            else if (trans.RuleID.Contains("Correct"))
                            {
                                // score & attempt
                                score = 1;
                                questionScore.Add(score.ToString());
                                questionDura.Add(duration.ToString());
                            }
                        }

                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 13);
            }
            return oneRecord;
        }


        // lesson 34
        public String getPerRecord34(String studentName, String lessonID)
        {
            String sectionLevel = "Medium", oneRecord = "";
            var db = new CSALDatabase(DB_URL);
            var oneTurn = db.FindTurns(lessonID, studentName);
            int lastTurnID = 99, attempCount = 0, thisAttempCount = 0;
            double score = 0, duration = 0;
            List<double> attempTime = new List<double>();
            List<String> questionScore = new List<string>();
            List<String> questionDura = new List<string>();

            if (oneTurn == null || oneTurn.Count < 1 || oneTurn[0].Turns.Count < 1)
            {
                return null;
            }
            else
            {
                // calculate total time of every Attempt
                foreach (var turn in oneTurn[0].Turns)
                {
                    if (turn.TurnID < lastTurnID)
                    {
                        attempCount++;
                        double turnDura = (int)turn.Duration;
                        turnDura = turnDura / 1000;
                        attempTime.Add(turnDura);
                    }
                    else
                    {
                        double turnDura = (int)turn.Duration;
                        attempTime[attempCount - 1] += turnDura / 1000;
                    }
                    lastTurnID = turn.TurnID;
                }

                lastTurnID = 0;
                foreach (var turn in oneTurn[0].Turns)
                {
                    // student tried more than 1, reset everything
                    if (turn.TurnID < lastTurnID)
                    {
                        oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 13);
                        thisAttempCount++;
                        score = 0;
                        duration = 0;
                        lastTurnID = 0;
                        questionScore.Clear();
                        questionDura.Clear();
                    }
                    else
                    {
                        duration = (int)turn.Duration;
                        duration = duration / 1000;

                        if (turn.Input.Event.ToString().Contains("Incorrect"))
                        {
                            score = 0;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }

                        else if (turn.Input.Event == "Correct")
                        {
                            // score & attempt
                            score = 1;
                            questionScore.Add(score.ToString());
                            questionDura.Add(duration.ToString());
                        }
                    }

                    lastTurnID = turn.TurnID;
                }
                oneRecord += (thisAttempCount + 1).ToString() + "\t" + attempTime[thisAttempCount].ToString() + "\t"
                            + fillRecord(questionScore, questionDura, 13);
            }
            return oneRecord;
        }

        // fill record with different question number
        public String fillRecord(List<String> questionScore, List<String> questionDura, int maxQuestionNum)
        {
            String halfRecord = "";
            int count = 0;
            if (questionScore == null || questionScore.Count < 1)
            {
                for (int i = 0; i < maxQuestionNum; i++)
                {
                    halfRecord += "\t";
                    count++;
                }
            }
            else
            {
                foreach (String quesScore in questionScore)
                {
                    halfRecord += quesScore + "\t";
                    count++;
                }
            }

            for (int i = 0; i < maxQuestionNum - count; i++) {
                halfRecord += "\t";
            }

            if (questionDura == null || questionDura.Count < 1)
            {
                for (int i = 0; i < maxQuestionNum; i++)
                {
                    halfRecord += "\t";
                    count++;
                }
            }
            else
            {
                foreach (String quesDura in questionDura)
                {
                    halfRecord += quesDura + "\t";
                }
            }

            for (int i = 0; i < maxQuestionNum - count; i++)
            {
                halfRecord += "\t";
            }

            return halfRecord; 
        }


        public String fixedNumberQ(List<String> questionScore)
        {
            String perRecord = "";
            foreach (String score in questionScore)
            {
                perRecord += score.ToString() + "\t"; 
            }
            return perRecord;
        }

        // get tag for each lesson
        public String getTag(int sectionNum)
        {
            String tags = "LessonAttempt" + "\t" + "TotalTime" + "\t";

            if (sectionNum == 1)
            {
                for (int i = 1; i < 10; i++)
                {
                    tags += "Q" + i.ToString() + "\t";
                }

                for (int i = 1; i < 10; i++)
                {
                    tags += "Q" + i.ToString() + "Time" + "\t";
                }
            }
            else if (sectionNum == 12)
            {
                // Medium
                for (int i = 1; i < 17; i++)
                {
                    tags += "Q" + i.ToString() + "\t";
                }

                // Medium
                for (int i = 1; i < 17; i++)
                {
                    tags += "Q" + i.ToString() + "Time" + "\t";
                }

                // Medium
                for (int i = 1; i < 17; i++)
                {
                    tags += "Q" + i.ToString() + "Second" + "\t";
                }

                // Medium
                for (int i = 1; i < 17; i++)
                {
                    tags += "Q" + i.ToString() + "SecondTime" + "\t";
                }
            }
            else if (sectionNum == 3)
            {
                // medium
                for (int i = 1; i < 11; i++)
                {
                    tags += "Q" + i.ToString() + "\t";
                }

                // Medium
                for (int i = 1; i < 11; i++)
                {
                    tags += "Q" + i.ToString() + "Time" + "\t";
                }
                
                // Easy
                for (int i = 1; i < 9; i++)
                {
                    tags += "EasyQ" + i.ToString() + "\t";
                }

                // Easy
                for (int i = 1; i < 9; i++)
                {
                    tags += "EasyQ" + i.ToString() + "Time" + "\t";
                }
                
                // Hard
                for (int i = 1; i < 12; i++)
                {
                    tags += "HardQ" + i.ToString() + "\t";
                }
               
                // Hard
                for (int i = 1; i < 12; i++)
                {
                    tags += "HardQ" + i.ToString() + "Time" + "\t";
                }
            }
            else if (sectionNum == 32)
            {
                // medium
                for (int i = 1; i < 12; i++)
                {
                    tags += "MediumQ" + i.ToString() + "\t";
                }

                // Medium
                for (int i = 1; i < 12; i++)
                {
                    tags += "MediumQ" + i.ToString() + "Time" + "\t";
                }

                // Medium
                for (int i = 1; i < 12; i++)
                {
                    tags += "MediumQ" + i.ToString() + "Second" + "\t";
                }

                // Medium
                for (int i = 1; i < 12; i++)
                {
                    tags += "MediumQ" + i.ToString() + "SecondTime" + "\t";
                }

                // Easy
                for (int i = 1; i < 11; i++)
                {
                    tags += "EasyQ" + i.ToString() + "\t";
                }

                // Easy
                for (int i = 1; i < 11; i++)
                {
                    tags += "EasyQ" + i.ToString() + "Time" + "\t";
                }

                // Easy
                for (int i = 1; i < 11; i++)
                {
                    tags += "EasyQ" + i.ToString() + "Second" + "\t";
                }

                // Easy
                for (int i = 1; i < 11; i++)
                {
                    tags += "EasyQ" + i.ToString() + "SecondTime" + "\t";
                }

                // Hard
                for (int i = 1; i < 11; i++)
                {
                    tags += "HardQ" + i.ToString() + "\t";
                }

                // Hard
                for (int i = 1; i < 11; i++)
                {
                    tags += "HardQ" + i.ToString() + "Time" + "\t";
                }


                // Hard
                for (int i = 1; i < 11; i++)
                {
                    tags += "HardQ" + i.ToString() + "Second" + "\t";
                }


                // Hard
                for (int i = 1; i < 11; i++)
                {
                    tags += "HardQ" + i.ToString() + "SecondTime" + "\t";
                }
            }
            return tags;
        }
    }
}
