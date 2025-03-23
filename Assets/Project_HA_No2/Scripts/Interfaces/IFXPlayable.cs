using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFXPlayable
{
    // ����Ʈ ��� ���� (��: ��ƼŬ �Ǵ� �ִϸ��̼� ���)
    void PlayEffect();
    // ����Ʈ ����� ��� ������ �� ȣ��Ǵ� �̺�Ʈ (�ʿ��� ���)
    event Action OnEffectFinished;
}
