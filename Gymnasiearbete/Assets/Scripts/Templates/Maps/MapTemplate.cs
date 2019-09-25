using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 0649

namespace ArenaShooter.Templates.Maps
{

    [CreateAssetMenu(menuName = "Templates/Map")]
    class MapTemplate : ScriptableObject
    {

        #region Editor

        [Header("Values")]
        [SerializeField] private byte   templateId;
        [SerializeField] private string sceneName;

        [Space]
        [SerializeField]           private string mapName;
        [SerializeField, TextArea] private string mapDescription;

        #endregion

        #region Getters

        public byte TemplateId
        {
            get
            {
                return templateId;
            }
        }

        public string SceneName
        {
            get
            {
                return sceneName;
            }
        }

        public Scene Scene
        {
            get
            {
                return SceneManager.GetSceneByName(sceneName);
            }
        }

        public string MapName
        {
            get
            {
                return mapName;
            }
        }

        public string MapDescription
        {
            get
            {
                return mapDescription;
            }
        }

        #endregion

    }

}
