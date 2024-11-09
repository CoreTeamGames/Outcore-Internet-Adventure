public static class UIManager
{
    static Window _currentWindow;
    public static Window CurrentWindow { get { return _currentWindow; } set { if (value != null) _currentWindow = value; } }

    public static void CloseWindow()
    {
        _currentWindow.Hide();
        if (_currentWindow.IsHidden)
            _currentWindow = null;
    }
}