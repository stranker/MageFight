using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILeaderboard : MonoBehaviour {

    public List<Texture2D> textureNumbers;
    public List<Image> numbers;

    public void ShowLeaderboard(int firstPlayerScore, int secondPlayerScore)
    {       
            Sprite n1 = Sprite.Create(textureNumbers[firstPlayerScore], new Rect(0.0f,0.0f, textureNumbers[0].width, textureNumbers[0].height), new Vector2(0.5f,0.5f));
            Sprite n2 = Sprite.Create(textureNumbers[secondPlayerScore + 4], new Rect(0.0f, 0.0f, textureNumbers[1].width, textureNumbers[1].height), new Vector2(0.5f, 0.5f));
            numbers[0].sprite = n1;
            numbers[1].sprite = n2;
    }
}
