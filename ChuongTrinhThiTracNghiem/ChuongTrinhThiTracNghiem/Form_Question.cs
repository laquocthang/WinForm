﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChuongTrinhThiTracNghiem
{
    public partial class Form_Question : Form
    {
        int remainingTime;
        string username;
        float score = 0;  //The user have gotten score
        int n = 0;      //The current index of list
        int t;          //Number questions in list
        bool isRandom = false;  //Check whether the list is random
        List_Question list = new List_Question();

        public Form_Question()
        {
            Form_Welcome frm = new Form_Welcome();
            frm.ShowDialog();
            InitializeComponent();
            username = frm.tbx_Ten.Text;
            remainingTime = int.Parse(frm.tbx_ThoiGian.Text);
            if (frm.chk_Random.Checked)
                isRandom = true;
        }

        private void Form_Question_Load(object sender, EventArgs e)
        {
            LoadQuestions();
            label1.Text = "Thí sinh: " + username;
            lbl_Remaining.Text = remainingTime + " phút";
            ShowQuestion();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            remainingTime--;
            lbl_Remaining.Text = remainingTime + " phút";
            if (remainingTime == 0)
            {
                MessageBox.Show("Hết giờ!!");
                timer1.Stop();
            }
        }

        /// <summary>
        /// Randomly shuffle a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        void LoadQuestions()
        {
            Microsoft_Excel xls = new Microsoft_Excel();
            List_Question lq = new List_Question();
            do
            {
                lq = xls.ToList();
                t = lq.list.Count;
                if (t == 0)
                    MessageBox.Show("Dữ liệu câu hỏi rỗng!!! Vui lòng chọn tập tin khác");
            } while (t == 0);
            if (isRandom)
                Shuffle(lq.list);
            list = lq;
        }

        void ShowQuestion()
        {
            Question q = list[n];
            groupBox1.Text = "Câu " + (n + 1) + " / " + t;
            lbl_CauHoi.Text = q.question;
            radioButton1.Text = q.answers[0];
            radioButton2.Text = q.answers[1];
            radioButton3.Text = q.answers[2];
            radioButton4.Text = q.answers[3];
            ShowYourAnswer();
        }

        void RecordYourAnswer()
        {
            if (radioButton1.Checked)
                list[n].selectedAns = 1;
            else if (radioButton2.Checked)
                list[n].selectedAns = 2;
            else if (radioButton3.Checked)
                list[n].selectedAns = 3;
            else if (radioButton4.Checked)
                list[n].selectedAns = 4;
        }

        void ShowYourAnswer()
        {
            if (list[n].selectedAns == 0)
                radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
            else if (list[n].selectedAns == 1)
                radioButton1.Checked = true;
            else if (list[n].selectedAns == 2)
                radioButton2.Checked = true;
            else if (list[n].selectedAns == 3)
                radioButton3.Checked = true;
            else radioButton4.Checked = true;
        }

        private void btn_Sau_Click(object sender, EventArgs e)
        {
            if (n < t)
            {
                RecordYourAnswer();
                n++;
                ShowQuestion();
            }
        }

        private void btn_Truoc_Click(object sender, EventArgs e)
        {
            if (n > 0)
            {
                RecordYourAnswer();
                n--;
                ShowQuestion();
            }
        }

        float GetResult()
        {
            float mark = 10f / t;
            foreach (var item in list.list)
            {
                if (item.correctAns == item.selectedAns)
                    score += mark;
            }
            return score;
        }

        private void Form_Question_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btn_NopBai_Click(object sender, EventArgs e)
        {
            GetResult();
            if (score == 10)
                MessageBox.Show("Thật tuyệt vời! Bạn được 10 điểm");
            else if (score <= 9 && score >= 8)
                MessageBox.Show("Tốt lắm, được " + score + " điểm");
            else if (score <= 7 && score >= 6)
                MessageBox.Show("Khá lắm, được " + score + " điểm");
            else MessageBox.Show(score + " điểm, cần cố gắng nhiều hơn");
        }
    }
}
