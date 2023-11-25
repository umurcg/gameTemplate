using UnityEngine;

namespace Helpers
{
    [AddComponentMenu("*Reboot/LevelDesign/Hollow Box")]
    public class HollowBox : MonoBehaviour
    {
        [SerializeField]public float width=1;
        [SerializeField]private float depth=1;
        [SerializeField]private float height=1;
        [SerializeField]private float wallDepth = .2f;
        [SerializeField]private Transform[] walls;
        [SerializeField]private GameObject ground;
        [SerializeField]private GameObject ceil;

        [SerializeField]public bool createGround;
        [SerializeField]public bool createCeil;

    
        private void CreateWalls()
        {
            walls = new Transform[4];

            for (int i = 0; i < 4; i++)
            {
                var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.transform.parent = transform;
                walls[i] = wall.transform;
            }
            
        
        }
    
        public void CreateGroundObject()
        {
            if (createGround && ground == null)
            {
                ground=GameObject.CreatePrimitive(PrimitiveType.Cube);
                ground.transform.parent = transform;
            }
        }
    
        public void CreateCeilObject()
        {
            if (createCeil && ceil == null)
            {
                ceil=GameObject.CreatePrimitive(PrimitiveType.Cube);
                ceil.transform.parent = transform;
            }
        }

        public void UpdateWalls()
        {
            if(walls==null || walls.Length!=4 || walls[0]==null) CreateWalls();
        
        
            Transform rightWall = walls[0];
            rightWall.localRotation=Quaternion.Euler(0,90,0);
            rightWall.transform.localPosition = new Vector3(width / 2+wallDepth/2, height/2, 0);
            rightWall.transform.localScale = new Vector3(depth+wallDepth*2, height, wallDepth);
        
            Transform leftWall = walls[1];
            leftWall.localRotation=Quaternion.Euler(0,90,0);
            leftWall.transform.localPosition = new Vector3(-width / 2-wallDepth/2, height/2, 0);
            leftWall.transform.localScale = new Vector3(depth+wallDepth*2, height, wallDepth);

        
            Transform forwardWall = walls[2];
            forwardWall.localRotation=Quaternion.Euler(0,0,0);
            forwardWall.transform.localPosition = new Vector3(0, height/2, -depth/2-wallDepth/2);
            forwardWall.transform.localScale = new Vector3(width+wallDepth*2, height, wallDepth);
        
            Transform backwardWall = walls[3];
            backwardWall.localRotation=Quaternion.Euler(0,0,0);
            backwardWall.transform.localPosition = new Vector3(0, height/2, depth/2+wallDepth/2);
            backwardWall.transform.localScale = new Vector3(width+wallDepth*2, height, wallDepth);
        
            if (ground)
            {
                var groundTransform = ground.transform;
                Vector3 groundPos = Vector3.zero;
                groundPos.y -= wallDepth/2;
                groundTransform.localScale = new Vector3(width + wallDepth, wallDepth, depth + wallDepth);
                groundTransform.localPosition = groundPos;
            }

            if (ceil)
            {
                var ceilTransform = ceil.transform;
                Vector3 ceilPos = new Vector3(0,height+wallDepth,0);
                ceilPos.y -= wallDepth/2;
                ceilTransform.localScale = new Vector3(width + wallDepth, wallDepth, depth + wallDepth);
                ceilTransform.localPosition = ceilPos;
            }

        
        }

        private void OnValidate()
        {
            if(Application.isPlaying) return;
        
            if(createCeil && !ceil) CreateCeilObject();
            if(createGround && !ground) CreateGroundObject();
            UpdateWalls();
        }
    }
}
