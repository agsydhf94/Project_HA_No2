using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HA
{
    public interface IVFXPlayable
    {
        // ����Ʈ ��� ���� (��: ��ƼŬ �Ǵ� �ִϸ��̼� ���)
        void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform parent = null, float customDuration = -1f);
        // ����Ʈ ����� ��� ������ �� ȣ��Ǵ� �̺�Ʈ (�ʿ��� ���)
        event Action OnEffectFinished;
    }
}
