﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class AccountControllerTestFixture
        {
            [
                Test,
                TestCase("abcd1234", false),
                TestCase("irf@uni-corvinus", false),
                TestCase("irf.uni-corvinus.hu", false),
                TestCase("irf@uni-corvinus.hu", true)
            ]
            public void TestValidateEmail(string email, bool expectedResult)
            {
                //Arrange
                var accountController = new AccountController();

                //Act
                var actualResult = accountController.ValidateEmail(email);

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }
    }
}
