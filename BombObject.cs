using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObject : CollectableItems
{
    public MobBase[] mobs;
    public float explosionTime = 2;
    public ParticleSystem ExplosionParticle;


    public override void OnCollect(Player player)
    {
        base.OnCollect(player);
        ScaleLoop();
    }

    IEnumerator BombSequence()
    {
        mobs = ExplosionBombAround();
        yield return new WaitForEndOfFrame();

        if (mobs.Length > 0)
        {
            foreach (var mob in mobs)
            {
                mob.owner.RemoveMobFromList(mob);
                mob.SetOwner(null);
                mob.transform.SetParent(null);
                mob.isActive = true;
                mob.AnimState = AnimStates.Run;
                mob.gameObject.layer = LayerMask.NameToLayer("Default");

            }

        }

        ExplosionParticle.Play();
        transform.DOKill();
        new SBF.Toolkit.DelayedAction(() => gameObject.SetActive(false), .5f).Execute(this);

    }

    void ScaleLoop()
    {
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
            .SetEase(Ease.InBounce).SetLoops(3).OnComplete(() => transform.DOKill()).OnComplete(() => {

                StartCoroutine(BombSequence());

            });

    }

    MobBase[] ExplosionBombAround()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 4);

        List<MobBase> _lsMobs = new List<MobBase>(0);
        for (int i = 0; i < cols.Length; i++)
        {
            MobBase mb = cols[i].GetComponent<MobBase>();

            if (mb != null)
                if (mb.owner != null)
                    if (!_lsMobs.Contains(mb))
                        _lsMobs.Add(mb);
        }
       
        return _lsMobs.ToArray();
    }

   
}
