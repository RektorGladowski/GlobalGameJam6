using System;

public interface IPopup<T>
{
    void OpenPopup(Action<T> callback);
}
