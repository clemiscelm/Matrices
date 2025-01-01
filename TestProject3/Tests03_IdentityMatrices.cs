﻿using NUnit.Framework;

namespace Maths_Matrices.Tests
{
    [TestFixture]
    public class Tests03_IdentityMatrices
    {
        [Test]
        public void TestGenerateIdentityMatrices()
        {
            MatrixInt identity2 = MatrixInt.Identity(2);
            Assert.AreEqual(new[,]
            {
                { 1, 0 },
                { 0, 1 },
            }, identity2.ToArray2D());

            MatrixInt identity3 = MatrixInt.Identity(3);
            Assert.AreEqual(new[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 },
            }, identity3.ToArray2D());

            MatrixInt identity4 = MatrixInt.Identity(4);
            Assert.AreEqual(new[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 },
            }, identity4.ToArray2D());
        }

        [Test]
        public void TestMatricesIsIdentity()
        {
            MatrixInt identity2 = new MatrixInt(new[,]
            {
                { 1, 0 },
                { 0, 1 }
            });
            identity2.IsIdentity();
            Assert.IsTrue(identity2.IsIdentity());

            MatrixInt identity3 = new MatrixInt(new[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            });
            Assert.IsTrue(identity3.IsIdentity());

            MatrixInt notSameColumnsAndLines = new MatrixInt(new[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 }
            });
            Assert.IsFalse(notSameColumnsAndLines.IsIdentity());

            MatrixInt notIdentity1 = new MatrixInt(new[,]
            {
                { 1, 0, 0 },
                { 0, 2, 0 },
                { 0, 0, 3 },
            });
            Assert.IsFalse(notIdentity1.IsIdentity());

            MatrixInt notIdentity2 = new MatrixInt(new[,]
            {
                { 1, 0, 4 },
                { 0, 1, 0 },
                { 0, 0, 1 },
            });
            Assert.IsFalse(notIdentity2.IsIdentity());
        }
    }
}