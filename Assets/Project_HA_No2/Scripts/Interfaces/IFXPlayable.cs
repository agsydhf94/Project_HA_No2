using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFXPlayable
{
    // 이펙트 재생 시작 (예: 파티클 또는 애니메이션 재생)
    void PlayEffect();
    // 이펙트 재생이 모두 끝났을 때 호출되는 이벤트 (필요한 경우)
    event Action OnEffectFinished;
}
