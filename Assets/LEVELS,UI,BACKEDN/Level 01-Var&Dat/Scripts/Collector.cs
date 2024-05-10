using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Collector : MonoBehaviour
{

    public static Collector instance;
    StringBuilder sb = new StringBuilder();
    public TMP_Text connect;
    public bool IsActive=false;

    private int position = 0;
    private int position2 = 0;

    private string level2Print= "printf( \" 1st element : %.2f \", mat1 );"+"\n"+ "printf( \" 2nd element : %e \", mat2 );"+"\n"+ "printf( \" 3rd element : %c \", mat3 );"+"\n"+ "printf( \" 4th element : %d \", mat4 );"+"\n"+ "\n" + "}";
    private string level3print = "printf( \" 1st element : %d \", mat1 )"+"\n"+ "printf( \" 2nd element : %c \", mat2 )"
        +"\n"+ "printf( \" 3rd element : %.2f \", mat3 )"+"\n"+ "printf( \" 4th element : %d \", mat4 )"+"\n"+ "printf( \" 5th element : %.1f \", mat5 )"+"\n"+ "\n" +"}";
  
    public float speed = 2.0f;
    public BoxCollider2D thiscollider;
    
    public SpriteRenderer CollectorImage;
    public Sprite offSprite;
    public Sprite onSprite;

    private void Awake()
    {
        instance = this;
        thiscollider.enabled = false; 
    }

    public void clear()
    {
        sb.Clear();
        IsActive= false;
        thiscollider.enabled = false;
        position = 0;
        CollectorImage.sprite=offSprite;
    }

    public void createCollector()
    {
        if (BoardManager.Instance.levelNumber==2) {
            thiscollider.enabled = true;
            CollectorImage.sprite = onSprite;
            if (!IsActive)
            {
                IsActive = true;
                if (IsActive)
                {
                    sb.Append("include<stdio.h>");
                    sb.Append("\n");
                    sb.Append("int main( )" + "\n" + " { ");
                    sb.Append("\n" + "\n");
                    sb.Append(connect.text);
                    sb.Append("\n" + "\n");
                    sb.Append(level2Print);
                    connect.text = "";
                    StartCoroutine(TypeText(sb.ToString()));
                }
            }
            return;
        }
       else if (BoardManager.Instance.levelNumber == 3)
        {
            thiscollider.enabled = true;
            CollectorImage.sprite = onSprite;
            if (!IsActive)
            {
                IsActive = true;
                if (IsActive)
                {
                    sb.Append("include<stdio.h>");
                    sb.Append("\n");
                    sb.Append("int main( )" + "\n" + " { ");
                    sb.Append("\n" + "\n");
                    sb.Append(connect.text);
                    sb.Append("\n" + "\n");
                    sb.Append(level3print);
                    connect.text = "";
                    StartCoroutine(TypeText(sb.ToString()));
                }
            }
            return;
        }
    }
    private IEnumerator TypeText(string fullText)
    {
        foreach (char character in fullText)
        {
            connect.text += character;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void addText(string material)
    {
        if (BoardManager.Instance.levelNumber == 2)
        {
            if (IsActive)
            {
                Debug.Log(position);
                if (position == 0)
                {
                    sb.Replace("mat1", material);
                    connect.text = sb.ToString();
                    position += 1;
                }
                else if (position == 1)
                {
                    sb.Replace("mat2", material);
                    connect.text = sb.ToString();
                    position += 1;
                }
                else if (position == 2)
                {
                    sb.Replace("mat3", material);
                    connect.text = sb.ToString();
                    position += 1;
                }
                else if (position == 3)
                {
                    sb.Replace("mat4", material);
                    connect.text = sb.ToString();
                }
            }
        }
        else if(BoardManager.Instance.levelNumber == 3)
        {
            Debug.Log(position);
            if (position2 == 0)
            {
                sb.Replace("mat1", material);
                connect.text = sb.ToString();
                position2 += 1;
            }
            else if (position2 == 1)
            {
                sb.Replace("mat2", material);
                connect.text = sb.ToString();
                position2 += 1;
            }
            else if (position2 == 2)
            {
                sb.Replace("mat3", material);
                connect.text = sb.ToString();
                position2 += 1;
            }
            else if (position2 == 3)
            {
                sb.Replace("mat4", material);
                connect.text = sb.ToString();
                position2 += 1;
            }
            else if (position2 == 3)
            {
                sb.Replace("mat4", material);
                connect.text = sb.ToString();
                position2 += 1;
            }
            else if (position2 == 4)
            {
                sb.Replace("mat5", material);
                connect.text = sb.ToString();
                position2 += 1;
            }
        }
    }

}
