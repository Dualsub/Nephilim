using OpenTK.Mathematics;

namespace Nephilim.Engine.Rendering
{
    public interface ICamera
    {
        Matrix4 GetViewMatrix();
        Matrix4 GetProjectionMatrix();
        bool IsProjectionMatrixDirty();
    }
}