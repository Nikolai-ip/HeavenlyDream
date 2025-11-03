using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Source.Common
{
    public class CommandList: ICommandMono
    {
        private List<ICommandMono> _commands;
        private void Awake()
        {
            _commands = GetComponentsInChildren<ICommandMono>().Skip(1).ToList();
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