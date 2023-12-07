using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction { get; private set; }
    public float speed = 30f;
    public float maxBounceAngle = 75f;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.direction = Vector2.right;
        }
        else this.direction = Vector2.zero;
    }


    void FixedUpdate()
    {
        if(this.direction != Vector2.zero)
        {
            this.rb.AddForce(this.direction * this.speed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {//amaç, topun paddle üzerinde nereye çarptığını tespit edip paddle'in üzerindeki çarptığı poziyonunu bulmak ve
         //çarptığı tarafa doğru yön vermek.
            Vector3 paddlePosition = this.transform.position; //paddle'imizin pozisyonunu aldık.
            Vector2 contactPoint = collision.GetContact(0).point; //temas noktası, 'GetContact(0).point' ilk temas noktası.
            float offset = paddlePosition.x - contactPoint.x;/*(topun tam olarak nereye çarptığı) okey ama mantığını anlayamadım.?"neden"?*/ Debug.Log(offset + " " + paddlePosition.x + " " + contactPoint.x);
            //anyway... paddle'ın toplam genişliğini bilmeliyiz, ortadan sağa ve sola yüzde olarak bölecez. en sol -100 en sağ 100 gibi.
            float width = collision.otherCollider.bounds.size.x / 2; //paddle'in genişliğini alıyoruz. bound.size.x, x ekseninde olan toplam genişliğini verir.
                                                                     //ve genişliğin yarısını almalıyız?? (yüzde hesabını yapabilmemiz için??)
            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rb.velocity); //topun mevcut açısını bulmamızı sağlar.Aşağıda açıklaması var.
            float bounceAngle = (offset / width) * this.maxBounceAngle; //yüzdesini aldık
                                                                        //topun paddle'dan çarptığı yöne doğru sekmesini iştediğimiz açı belirleme
                                                                        //ve en fazla 'maxBounceAngle' sekebileceği açı. '75'
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle);
            //topun -75 veya 75 i aşmamasını sağlamış olan yeni açısı.
            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            ball.rb.velocity = rotation * Vector3.up * ball.rb.velocity.magnitude;
        } //Vector2.SignedAngle(Vector2.up, ball.rb.velocity); nedir? unity documentation https://docs.unity3d.com/ScriptReference/Vector3.SignedAngle.html
          //İki vektör arasındaki olası iki açıdan daha küçük olanı döndürülür, bu nedenle sonuç asla
          //180 dereceden büyük veya -180 dereceden küçük olmaz. Başlangıç ve bitiş vektörlerini bir kağıt parçası üzerinde,
          //her ikisi de aynı noktadan çıkan çizgiler olarak hayal ederseniz, eksen vektörü kağıttan yukarıyı gösterecektir.
          //İki vektör arasındaki ölçülen açı, saat yönünde pozitif ve saat yönünün tersine negatif olacaktır.
    }

    public void ResetPaddle()
    {
        this.transform.position = new Vector2(0f,this.transform.position.y);
        this.rb.velocity = Vector2.zero;
    }

}
