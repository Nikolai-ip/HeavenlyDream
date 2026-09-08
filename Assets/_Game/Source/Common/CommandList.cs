using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Common
{
    public class CommandList: ICommandMono
    {
        [SerializeField] [ReadOnly] private List<ICommandMono> _commands;
        private void Awake()
        {
            _commands = GetComponentsFromDirectChildren();
        }
        List<ICommandMono>  GetComponentsFromDirectChildren()
        {
            var result = new List<ICommandMono>();

            foreach (Transform child in transform)
            {
                var component = child.GetComponent<ICommandMono>();
                if (component != null)
                    result.Add(component);
            }

            return result;
        }

        public override void Execute()
        {
            foreach (var command in _commands)
            {
                try
                {
                    command.Execute();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}