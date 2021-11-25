﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            [
                Test,
                TestCase("gkkojw", false),
                TestCase("GKKOJW", false),
                TestCase("gkkojw", false),
                TestCase("rovid7", false),
                TestCase("Megfelelo7", true)
            ]

            public bool ValidatePassword (string pw)
            {
                var lowerCase = new Regex(@"[a-z]+");
                var upperCase = new Regex(@"[A-Z]+");
                var number = new Regex(@"[0-9]+");
                var nyolcHosszu = new Regex(@".{8,}");
                return lowerCase.IsMatch(pw) && upperCase.IsMatch(pw) && number.IsMatch(pw) && nyolcHosszu.IsMatch(pw);
            }

            [
                Test,
                TestCase("gkkojw@uni-corvinus.hu", "Megfelelo7"),
                TestCase("gkkojw@uni-corvinus.hu", "Megfelelo45678"),
             ]
            public void TestRegisterHappyPath(string email, string password)
            {
                //Arrange
                var accountController = new AccountController();

                //Act
                var actualResult = accountController.Register(email, password);

                //Assert
                Assert.AreEqual(email, actualResult.Email);
                Assert.AreEqual(password, actualResult.Password);
                Assert.AreNotEqual(Guid.Empty, actualResult.ID);
            }
        }
    }
}
