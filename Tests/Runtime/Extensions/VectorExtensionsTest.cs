using System.Collections;
using KalkuzSystems.Utility.Extensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Kalkuz.Utility.Tests
{
    public class VectorExtensionsTest
    {
        [UnityTest]
        public IEnumerator TestProjectXY()
        {   
            var vector = new Vector3(1, 2, 3);
            var projected = vector.ProjectXY();
            
            Assert.AreEqual(projected, new Vector3(1, 2));
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestProjectXZ()
        {
            var vector = new Vector3(1, 2, 3);
            var projected = vector.ProjectXZ();
            
            Assert.AreEqual(projected, new Vector3(1, 0, 3));
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestProjectYZ()
        {
            var vector = new Vector3(1, 2, 3);
            var projected = vector.ProjectYZ();
            
            Assert.AreEqual(projected, new Vector3(0, 2, 3));
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestAngleBetweenXY()
        {
            var a = new Vector3(1, 0);
            var b = new Vector3(0, 1);
            var c = new Vector3(1, 1);
            var d = new Vector3(0, 0);
            
            var angle = a.RotationAngleXY(b, true);
            Assert.AreEqual(angle, 135);
            
            angle = a.RotationAngleXY(c, true);
            Assert.AreEqual(angle, 90);
            
            angle = a.RotationAngleXY(d, true);
            Assert.AreEqual(angle, 180);
            
            angle = b.RotationAngleXY(c, true);
            Assert.AreEqual(angle, 0);
            
            angle = b.RotationAngleXY(d, true);
            Assert.AreEqual(angle, 270);
            
            angle = c.RotationAngleXY(d, true);
            Assert.AreEqual(angle, 225);
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestAngleBetweenXZ()
        {
            var a = new Vector3(1, 0, 0);
            var b = new Vector3(0, 0, 1);
            var c = new Vector3(1, 0, 1);
            var d = new Vector3(0, 0, 0);
            
            var angle = a.RotationAngleXZ(b, true);
            Assert.AreEqual(angle, 135);
            
            angle = a.RotationAngleXZ(c, true);
            Assert.AreEqual(angle, 90);
            
            angle = a.RotationAngleXZ(d, true);
            Assert.AreEqual(angle, 180);
            
            angle = b.RotationAngleXZ(c, true);
            Assert.AreEqual(angle, 0);
            
            angle = b.RotationAngleXZ(d, true);
            Assert.AreEqual(angle, 270);
            
            angle = c.RotationAngleXZ(d, true);
            Assert.AreEqual(angle, 225);
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator TestAngleBetweenYZ()
        {
            var a = new Vector3(0, 0, 1);
            var b = new Vector3(0, 1, 0);
            var c = new Vector3(0, 1, 1);
            var d = new Vector3(0, 0, 0);
            
            var angle = a.RotationAngleYZ(b, true);
            Assert.AreEqual(angle, 135);
            
            angle = a.RotationAngleYZ(c, true);
            Assert.AreEqual(angle, 90);
            
            angle = a.RotationAngleYZ(d, true);
            Assert.AreEqual(angle, 180);
            
            angle = b.RotationAngleYZ(c, true);
            Assert.AreEqual(angle, 0);
            
            angle = b.RotationAngleYZ(d, true);
            Assert.AreEqual(angle, 270);
            
            angle = c.RotationAngleYZ(d, true);
            Assert.AreEqual(angle, 225);
            
            yield return null;
        }
    }
}