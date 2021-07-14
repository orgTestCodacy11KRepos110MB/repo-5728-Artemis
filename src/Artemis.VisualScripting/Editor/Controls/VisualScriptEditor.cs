﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Artemis.Core.VisualScripting;
using Artemis.VisualScripting.Model;

namespace Artemis.VisualScripting.Editor.Controls
{
    public class VisualScriptEditor : Control
    {
        #region Properties & Fields

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register(
            "Script", typeof(IScript), typeof(VisualScriptEditor), new PropertyMetadata(default(IScript)));

        public IScript Script
        {
            get => (IScript)GetValue(ScriptProperty);
            set => SetValue(ScriptProperty, value);
        }

        public static readonly DependencyProperty AvailableNodesProperty = DependencyProperty.Register(
            "AvailableNodes", typeof(IEnumerable<NodeData>), typeof(VisualScriptEditor), new PropertyMetadata(default(IEnumerable<NodeData>)));

        public IEnumerable<NodeData> AvailableNodes
        {
            get => (IEnumerable<NodeData>)GetValue(AvailableNodesProperty);
            set => SetValue(AvailableNodesProperty, value);
        }

        #endregion

        #region Methods

        #endregion
    }
}
