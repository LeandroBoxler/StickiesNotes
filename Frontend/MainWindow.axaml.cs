using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StickieNotes.View;

namespace StickieNotes;

public partial class MainWindow : Window
{
    private readonly List<Note> openNotes = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void CreateNoteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var noteWindow = new Note();

        noteWindow.Closed += OnNoteClosed;

        openNotes.Add(noteWindow);
        noteWindow.Show();
    }

    private void OnNoteClosed(object? sender, EventArgs e)
    {
        if (sender is not Note closedNote)
        {
            return;
        }

        closedNote.Closed -= OnNoteClosed;
        openNotes.Remove(closedNote);
    }
}