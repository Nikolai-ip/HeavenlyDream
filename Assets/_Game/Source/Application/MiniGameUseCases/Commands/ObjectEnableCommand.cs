using _Game.Source.Common;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Commands
{
    public class ObjectEnableCommand: ICommandMono
    {
        [SerializeField] private GameObject _object;
        [SerializeField] private bool _enable;
        public override void Execute()
        {
            _object.SetActive(_enable);
        }
    }
}