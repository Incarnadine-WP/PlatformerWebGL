using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KeySymbol : MonoBehaviour
{
    [SerializeField] private List<KeySymbolUI> _keySymbolUI;
    [SerializeField] private ParticleSystem _vfxPrefab;

    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (player != null)
            {
                var emptyKeySymbolUI = _keySymbolUI.FirstOrDefault(x => x.Image.color == x.StartColor);
                if (emptyKeySymbolUI != null)
                    emptyKeySymbolUI.CollectSymbolAnim();

                player.OnTakeKeySymbol?.Invoke();
                PlayVFX();
                SoundManager.Instance.PlayTakeSymbolSound(player.transform.position);

                gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator SymbolBoxColliderEnable()
    {
        var delay = new WaitForSeconds(1f);
        yield return delay;
        _boxCollider2D.enabled = true;
    }

    private void PlayVFX()
    {
        _vfxPrefab.gameObject.transform.parent = null;
        _vfxPrefab.Play();
        Destroy(_vfxPrefab.gameObject, _vfxPrefab.main.duration);
    }
}
