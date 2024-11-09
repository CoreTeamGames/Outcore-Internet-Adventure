using System.Runtime.InteropServices; // Для импорта функций WinApi
using System.Text; // Для работы со строками
using UnityEditor; // Для работы с окном редактора
using UnityEditor.UIElements;
using UnityEngine; // Для работы с Unity
using UnityEngine.UIElements;

// Класс для хранения данных
public class Achievement
{
    public bool isSecret; // Секретность
    public int id; // Идентификатор
    public string titleKey; // Ключ заголовка
    public string descriptionKey; // Ключ описания
    public Sprite achievementIcon; // Иконка

    // Конструктор по умолчанию
    public Achievement()
    {
        isSecret = false;
        id = 0;
        titleKey = "";
        descriptionKey = "";
        achievementIcon = null;
    }

    // Конструктор с параметрами
    public Achievement(bool isSecret, int id, string titleKey, string descriptionKey, Sprite achievementIcon)
    {
        this.isSecret = isSecret;
        this.id = id;
        this.titleKey = titleKey;
        this.descriptionKey = descriptionKey;
        this.achievementIcon = achievementIcon;
    }
}

// Класс для окна редактора
public class AchievementEditor : EditorWindow
{
    // Путь к .ini файлу
    private string iniFile = "achievements.ini";

    // Импорт функции GetPrivateProfileString из WinApi
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string filePath);

    // Импорт функции WritePrivateProfileString из WinApi
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

    // Поле для хранения экземпляра класса Achievement
    private Achievement achievement;

    // Элементы управления для ввода и вывода данных
    private Toggle isSecretToggle;
    private IntegerField idField;
    private TextField titleKeyField;
    private TextField descriptionKeyField;
    private ObjectField achievementIconField;
    private Button loadButton;
    private Button saveButton;

    // Метод, создающий элемент меню для открытия окна редактора
    [MenuItem("Window/Achievement Editor")]
    public static void ShowWindow()
    {
        // Открываем окно редактора
        GetWindow<AchievementEditor>("Achievement Editor");
    }

    // Метод, вызываемый при создании окна редактора
    private void OnEnable()
    {
        // Создаем экземпляр класса Achievement
        achievement = new Achievement();

        // Создаем элементы управления
        isSecretToggle = new Toggle("Is Secret");
        idField = new IntegerField("ID");
        titleKeyField = new TextField("Title Key");
        descriptionKeyField = new TextField("Description Key");
        achievementIconField = new ObjectField("Achievement Icon");
        achievementIconField.objectType = typeof(Sprite);
        loadButton = new Button(LoadAchievement) { text = "Load" };
        saveButton = new Button(SaveAchievement) { text = "Save" };
    }

    // Метод, вызываемый при отрисовке окна редактора
    private void OnGUI()
    {
        // Добавляем элементы управления в окно редактора
        isSecretToggle.value = achievement.isSecret;
        idField.value = achievement.id;
        titleKeyField.value = achievement.titleKey;
        descriptionKeyField.value = achievement.descriptionKey;
        achievementIconField.value = achievement.achievementIcon;
        loadButton.SetEnabled(true);
        saveButton.SetEnabled(true);

        rootVisualElement.Add(isSecretToggle);
        rootVisualElement.Add(idField);
        rootVisualElement.Add(titleKeyField);
        rootVisualElement.Add(descriptionKeyField);
        rootVisualElement.Add(achievementIconField);
        rootVisualElement.Add(loadButton);
        rootVisualElement.Add(saveButton);
    }

    // Метод для загрузки данных из .ini файла в экземпляр класса Achievement
    private void LoadAchievement()
    {
        // Создаем объект StringBuilder для хранения считанных значений
        StringBuilder value = new StringBuilder(255);

        // Считываем значение isSecret из секции Achievement и ключа isSecret
        GetPrivateProfileString("Achievement", "isSecret", "", value, 255, iniFile);
        // Преобразуем значение isSecret в логическое значение и присваиваем полю isSecret
        achievement.isSecret = bool.Parse(value.ToString());

        // Считываем значение id из секции Achievement и ключа id
        GetPrivateProfileString("Achievement", "id", "", value, 255, iniFile);
        // Преобразуем значение id в целое число и присваиваем полю id
        achievement.id = int.Parse(value.ToString());

        // Считываем значение titleKey из секции Achievement и ключа titleKey
        GetPrivateProfileString("Achievement", "titleKey", "", value, 255, iniFile);
        // Присваиваем значение titleKey полю titleKey
        achievement.titleKey = value.ToString();

        // Считываем значение descriptionKey из секции Achievement и ключа descriptionKey
        GetPrivateProfileString("Achievement", "descriptionKey", "", value, 255, iniFile);
        // Присваиваем значение descriptionKey полю descriptionKey
        achievement.descriptionKey = value.ToString();

        // Считываем значение achievementIcon из секции Achievement и ключа achievementIcon
        GetPrivateProfileString("Achievement", "achievementIcon", "", value, 255, iniFile);
        // Преобразуем значение achievementIcon в путь к ресурсу и загружаем спрайт
        achievement.achievementIcon = Resources.Load<Sprite>(value.ToString());

        // Выводим сообщение об успешной загрузке
        Debug.Log("Achievement loaded from " + iniFile);
    }

    // Метод для сохранения данных из экземпляра класса Achievement в .ini файл
    private void SaveAchievement()
    {
        // Записываем значение isSecret из поля isSecret в секцию Achievement и ключ isSecret
        WritePrivateProfileString("Achievement", "isSecret", achievement.isSecret.ToString(), iniFile);

        // Записываем значение id из поля id в секцию Achievement и ключ id
        WritePrivateProfileString("Achievement", "id", achievement.id.ToString(), iniFile);

        // Записываем значение titleKey из поля titleKey в секцию Achievement и ключ titleKey
        WritePrivateProfileString("Achievement", "titleKey", achievement.titleKey, iniFile);

        // Записываем значение descriptionKey из поля descriptionKey в секцию Achievement и ключ descriptionKey
        WritePrivateProfileString("Achievement", "descriptionKey", achievement.descriptionKey, iniFile);

        // Записываем значение achievementIcon из поля achievementIcon в секцию Achievement и ключ achievementIcon
        // Преобразуем спрайт в путь к ресурсу
        WritePrivateProfileString("Achievement", "achievementIcon", AssetDatabase.GetAssetPath(achievement.achievementIcon), iniFile);

        // Выводим сообщение об успешном сохранении
        Debug.Log("Achievement saved to " + iniFile);
    }
}