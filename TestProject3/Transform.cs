namespace Maths_Matrices.Tests
{
    public class Transform
    {
        private const float DegreesToRadians = (float)Math.PI / 180f;
        private Vector3 localPosition;
        private Vector3 localRotation;
        private Vector3 localScale;

        private MatrixFloat localTranslationMatrix;
        private MatrixFloat localRotationXMatrix;
        private MatrixFloat localRotationYMatrix;
        private MatrixFloat localRotationZMatrix;
        private MatrixFloat localScaleMatrix;

        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                UpdateLocalTranslationMatrix();
            }
        }

        public Vector3 LocalRotation
        {
            get => localRotation;
            set
            {
                localRotation = value;
                UpdateLocalRotationMatrices();
            }
        }

        public Vector3 LocalScale
        {
            get => localScale;
            set
            {
                localScale = value;
                UpdateLocalScaleMatrix();
            }
        }

        public MatrixFloat LocalToWorldMatrix => CalculateLocalToWorldMatrix();
        public MatrixFloat WorldToLocalMatrix => CalculateWorldToLocalMatrix();

        public MatrixFloat LocalTranslationMatrix => localTranslationMatrix;
        public MatrixFloat LocalRotationXMatrix => localRotationXMatrix;
        public MatrixFloat LocalRotationYMatrix => localRotationYMatrix;
        public MatrixFloat LocalRotationZMatrix => localRotationZMatrix;
        public MatrixFloat LocalRotationMatrix => CalculateLocalRotationMatrix();
        public MatrixFloat LocalScaleMatrix => localScaleMatrix;

        public Transform()
        {
            localPosition = new Vector3(0f, 0f, 0f);
            localRotation = new Vector3(0f, 0f, 0f);
            localScale = new Vector3(1f, 1f, 1f);
            localTranslationMatrix = CreateTranslationMatrix(localPosition);
            localRotationXMatrix = CreateRotationXMatrix(localRotation.x);
            localRotationYMatrix = CreateRotationYMatrix(localRotation.y);
            localRotationZMatrix = CreateRotationZMatrix(localRotation.z);
            localScaleMatrix = CreateScaleMatrix(localScale);
        }

        private void UpdateLocalTranslationMatrix()
        {
            localTranslationMatrix = CreateTranslationMatrix(localPosition);
        }

        private void UpdateLocalRotationMatrices()
        {
            localRotationXMatrix = CreateRotationXMatrix(localRotation.x);
            localRotationYMatrix = CreateRotationYMatrix(localRotation.y);
            localRotationZMatrix = CreateRotationZMatrix(localRotation.z);
        }

        private void UpdateLocalScaleMatrix()
        {
            localScaleMatrix = CreateScaleMatrix(localScale);
        }

        private MatrixFloat CreateTranslationMatrix(Vector3 position)
        {
            return new MatrixFloat(new[,]
            {
                { 1f, 0f, 0f, position.x },
                { 0f, 1f, 0f, position.y },
                { 0f, 0f, 1f, position.z },
                { 0f, 0f, 0f, 1f },
            });
        }

        

        private MatrixFloat CreateRotationYMatrix(float angle)
        {
            float radians = angle * (float)Math.PI / 180f;
            float cosA = (float)Math.Cos(radians);
            float sinA = (float)Math.Sin(radians);

            return new MatrixFloat(new[,]
            {
                { cosA, 0f, sinA, 0f },
                { 0f, 1f, 0f, 0f },
                { -sinA, 0f, cosA, 0f },
                { 0f, 0f, 0f, 1f },
            });
        }

        private MatrixFloat CreateRotationZMatrix(float angle)
        {
            float radians = angle * (float)Math.PI / 180f;
            float cosA = (float)Math.Cos(radians);
            float sinA = (float)Math.Sin(radians);

            return new MatrixFloat(new[,]
            {
                { cosA, -sinA, 0f, 0f },
                { sinA, cosA, 0f, 0f },
                { 0f, 0f, 1f, 0f },
                { 0f, 0f, 0f, 1f },
            });
        }

        private MatrixFloat CreateScaleMatrix(Vector3 scale)
        {
            return new MatrixFloat(new[,]
            {
                { scale.x, 0f, 0f, 0f },
                { 0f, scale.y, 0f, 0f },
                { 0f, 0f, scale.z, 0f },
                { 0f, 0f, 0f, 1f },
            });
        }
        

        private MatrixFloat CalculateLocalRotationMatrix()
        {
            return localRotationZMatrix * localRotationYMatrix * localRotationXMatrix;
        }

        
        private MatrixFloat CalculateLocalToWorldMatrix()
        {
            return LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix;
        }

        private MatrixFloat CalculateWorldToLocalMatrix()
        {
            return LocalToWorldMatrix.InvertByRowReduction();
        }

        private MatrixFloat CreateRotationXMatrix(float angle)
        {
            float radians = angle * DegreesToRadians;
            float cosA = (float)Math.Cos(radians);
            float sinA = (float)Math.Sin(radians);

            return new MatrixFloat(new[,]
            {
                { 1f, 0f, 0f, 0f },
                { 0f, cosA, -sinA, 0f },
                { 0f, sinA, cosA, 0f },
                { 0f, 0f, 0f, 1f },
            });
        }
    }

    public struct Vector3
    {
        public float x, y, z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
