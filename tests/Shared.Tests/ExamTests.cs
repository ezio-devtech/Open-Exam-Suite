﻿using System;
using System.IO;
using System.Drawing;
using Shared.Util;
using Xunit;

namespace Shared.Tests
{
    public class ExamTests
    {
        Exam exam = new Exam
        {
            Properties = new Properties
            {
                Title = "Test",
                Version = 3,
                Code = "T01",
                Instructions = "Goodluck! Make good use of your time.",
                Passmark = 650,
                TimeLimit = 5
            },
            Sections = new System.Collections.Generic.List<Section>
            {
                new Section
                {
                    Title = "Section A",
                    Questions = new System.Collections.Generic.List<Question>
                    {
                        new Question
                        {
                            No = 1,
                            Text = "Question 1",
                            Answer = 'A',
                            Options = new System.Collections.Generic.List<Option>
                            {
                                new Option
                                {
                                    Text = "Option 1",
                                    Alphabet = 'A'
                                },
                                new Option
                                {
                                    Text = "Option 2",
                                    Alphabet = 'B'
                                }
                            },
                            Image = new Bitmap("test.png")
                        },
                        new Question
                        {
                            No = 1,
                            Text = "Question 2",
                            Answer = 'B',
                            Options = new System.Collections.Generic.List<Option>
                            {
                                new Option
                                {
                                    Text = "Option 1",
                                    Alphabet = 'A'
                                },
                                new Option
                                {
                                    Text = "Option 2",
                                    Alphabet = 'B'
                                }
                            }
                        }
                    }
                }
            }
        };

        [Fact]
        public void ExamGetsSerialized()
        {
            string filepath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "test.oef";
            Reader.WriteExamToOefFile(exam, filepath, true);
            Assert.Equal(true, File.Exists(filepath));
        }

        [Fact]
        public void ExamGetsDeserialized()
        {
            string filepath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "test.oef";
            Exam exam = Reader.FromOefFile(filepath, true);
            Assert.NotNull(exam);
        }

        [Fact]
        public void NullExamPassed()
        {
            Exam nullExam = null;
            Assert.Throws<NullReferenceException>(() => { Reader.WriteExamToOefFile(nullExam, @"C:\"); });
        }

        [Fact]
        public void EmptyFilePath()
        {
            Assert.Throws<ArgumentException>(() => { Reader.WriteExamToOefFile(exam, string.Empty); });
        }
    }
}
