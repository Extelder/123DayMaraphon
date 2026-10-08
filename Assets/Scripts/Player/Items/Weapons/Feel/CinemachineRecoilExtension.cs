using Cinemachine;
using UnityEngine;

// Визуальный кик камеры поверх POV и импульсов: доворот ориентации и FOV-панч.
// Значения выставляет WeaponFeel, сама базовая настройка линзы (PlayerFOV, PlayerDashFOV) не меняется.
public class CinemachineRecoilExtension : CinemachineExtension
{
    public Vector3 KickRotation { get; set; }
    public float FovOffset { get; set; }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Finalize)
            return;

        if (KickRotation != Vector3.zero)
            state.OrientationCorrection *= Quaternion.Euler(KickRotation);

        if (!Mathf.Approximately(FovOffset, 0f))
        {
            LensSettings lens = state.Lens;
            lens.FieldOfView = Mathf.Clamp(lens.FieldOfView + FovOffset, 1f, 179f);
            state.Lens = lens;
        }
    }
}
