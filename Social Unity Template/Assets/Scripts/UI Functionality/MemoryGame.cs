using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemoryGame : MonoBehaviour
{

    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite defaultSprite;

    [SerializeField] private AudioClip flipClip1; 
    [SerializeField] private AudioClip flipClip2; 
    [SerializeField] private AudioClip failClip;

    private Dictionary<Button, Sprite> _combinations = new Dictionary<Button, Sprite>();
    private Dictionary<Sprite, int> _usages = new Dictionary<Sprite, int>();

    private int _currentlyViewing = 0;
    private Button _last;
    private Button _current;
    private bool _locked;
    
    void OnEnable()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            _usages.Add(sprites[i], 2);
        }
        
        for (int i = 0; i < buttons.Length; i++)
        {
            Button current = buttons[i];
            current.onClick.AddListener(() => ButtonClicked(current));
            int spriteIndex = Random.Range(0, sprites.Length);
            if (_usages[sprites[spriteIndex]] > 0)
            {
                _usages[sprites[spriteIndex]]--;
                _combinations.Add(current, sprites[spriteIndex]);
            }
            else
            {
                for (int step = 0; step < sprites.Length; step++)
                {
                    if (_usages[sprites[(spriteIndex+step)%sprites.Length]] > 0)
                    {
                        _usages[sprites[(spriteIndex+step)%sprites.Length]]--;
                        _combinations.Add(current, sprites[(spriteIndex+step)%sprites.Length]);
                        break;
                    }
                }
            }
        }
    }

    private void ButtonClicked(Button button)
    {
        if(_usages[_combinations[button]] != 2 && !_locked) {
            if (_currentlyViewing == 0)
            {
                _currentlyViewing++;
                _last = button;
                button.image.sprite = _combinations[button];
                //SoundManagement.Instance.PlaySound(flipClip1);
            } else if (_currentlyViewing == 1)
            {
                button.image.sprite = _combinations[button];
                //SoundManagement.Instance.PlaySound(flipClip1);
                _current = button;
                
                if (_combinations[button] == _combinations[_last])
                {
                    _usages[_combinations[button]] = 2;
                    bool finished = true;
                    for (int i = 0; i < sprites.Length; i++)
                    {
                        if (_usages[sprites[i]] != 2)
                        {
                            finished = false;
                        }
                    }

                    if (finished)
                    {
                        gameObject.Destroy();
                        GameObject.Find("MemoryGameUI").GetComponent<MemoryGameUIManager>().DamageSafe();
                    }
                }
                else
                {
                    _locked = true;
                    //SoundManagement.Instance.PlaySound(failClip);
                    Invoke(nameof(HideSprite), 0.5f);
                }
                
                _currentlyViewing = 0;
            }
        }
    }

    private void HideSprite()
    {
        _current.image.sprite = defaultSprite;
        _last.image.sprite = defaultSprite;
        //SoundManagement.Instance.PlaySound(flipClip2);
        _locked = false;
    }
}

