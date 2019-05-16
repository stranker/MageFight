using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctructibleTest : MonoBehaviour {

	public int levelOfDestruction = 1;
	public int maxLevelOfDestruction = 4;
	public int minPiecesPerLevel = 2;
	public int maxPiecesPerLevel = 3;
	public GameObject piecePrefab;
	public Sprite[] spritePieces;
	public bool canBeDestroyed = false;
	public float destructionTime;
	public float timer = 0;
	public Color pieceColor;
	
	private void Update(){
		if(!canBeDestroyed){
			timer += Time.deltaTime;
			if(timer >= destructionTime){
				canBeDestroyed = true;
				timer = 0;
			}
		}
		else{
			if(levelOfDestruction >= maxLevelOfDestruction){
				timer += Time.deltaTime;
				if(timer >= 3)
					Destroy(gameObject);
			}
		}
	}
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spell" && levelOfDestruction < maxLevelOfDestruction && canBeDestroyed)
        {
			int pieces = UnityEngine.Random.Range(minPiecesPerLevel,maxPiecesPerLevel);
			for(int i = 0; i < pieces; i++){
				GameObject piece = Instantiate(piecePrefab, transform.position, Quaternion.identity, transform.parent);
				piece.GetComponent<SpriteRenderer>().sprite = spritePieces[levelOfDestruction -1];
				piece.AddComponent<PolygonCollider2D>();
				piece.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-15,15),15);
				piece.AddComponent<DesctructibleTest>();
				piece.GetComponent<DesctructibleTest>().canBeDestroyed = false;
				piece.GetComponent<DesctructibleTest>().destructionTime = destructionTime; 
				piece.GetComponent<DesctructibleTest>().levelOfDestruction = levelOfDestruction + 1;
				piece.GetComponent<DesctructibleTest>().spritePieces = spritePieces;
				piece.GetComponent<DesctructibleTest>().piecePrefab = piecePrefab;
				piece.GetComponent<DesctructibleTest>().pieceColor = pieceColor;
				piece.GetComponent<SpriteRenderer>().color = pieceColor;
			}
			Destroy(gameObject);
		}
    }
}
