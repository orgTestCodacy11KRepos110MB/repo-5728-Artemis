﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Artemis.UI.Shared.Services.ProfileEditor.Commands;

/// <summary>
///     Represents a profile editor command that can be used to combine multiple commands into one.
/// </summary>
public class CompositeCommand : IProfileEditorCommand, IDisposable
{
    private readonly List<IProfileEditorCommand> _commands;
    private bool _ignoreNextExecute;

    /// <summary>
    ///     Creates a new instance of the <see cref="CompositeCommand" /> class.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    /// <param name="displayName">The display name of the composite command.</param>
    public CompositeCommand(IEnumerable<IProfileEditorCommand> commands, string displayName)
    {
        if (commands == null)
            throw new ArgumentNullException(nameof(commands));
        _commands = commands.ToList();
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="CompositeCommand" /> class.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    /// <param name="displayName">The display name of the composite command.</param>
    /// <param name="ignoreFirstExecute">Whether or not to ignore the first execute because commands are already executed</param>
    internal CompositeCommand(IEnumerable<IProfileEditorCommand> commands, string displayName, bool ignoreFirstExecute)
    {
        if (commands == null)
            throw new ArgumentNullException(nameof(commands));

        _ignoreNextExecute = ignoreFirstExecute;
        _commands = commands.ToList();
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (IProfileEditorCommand profileEditorCommand in _commands)
        {
            if (profileEditorCommand is IDisposable disposable)
                disposable.Dispose();
        }
    }

    #region Implementation of IProfileEditorCommand

    /// <inheritdoc />
    public string DisplayName { get; }

    /// <inheritdoc />
    public void Execute()
    {
        if (_ignoreNextExecute)
        {
            _ignoreNextExecute = false;
            return;
        }

        foreach (IProfileEditorCommand profileEditorCommand in _commands)
            profileEditorCommand.Execute();
    }

    /// <inheritdoc />
    public void Undo()
    {
        // Undo in reverse by iterating from the back
        for (int index = _commands.Count - 1; index >= 0; index--)
            _commands[index].Undo();
    }

    #endregion
}