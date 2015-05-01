﻿using Microsoft.DataTransfer.WpfHost.Basics;
using Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.EditableItemsList;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.DataTransfer.WpfHost.Extensibility.Basics.Controls.FilesPicker
{
    sealed class FilesPickerViewModel : BindableBase
    {
        private EditItemsCollectionCommandBase<string> addFiles;
        private EditItemsCollectionCommandBase<string> addSingleFolder;
        private EditItemsCollectionCommandBase<string> addRecursiveFolder;
        private EditItemsCollectionCommandBase<string> addUrl;
        private EditItemsCollectionCommandBase<string> removeFiles;

        private IList<string> files;

        public IList<string> Files
        {
            get { return files; }
            set
            {
                SetProperty(ref files, value);
                addFiles.Items = addSingleFolder.Items = addUrl.Items =
                    addRecursiveFolder.Items = removeFiles.Items = files;
            }
        }

        public ICommand AddFiles
        {
            get { return addFiles; }
        }

        public ICommand AddSingleFolder
        {
            get { return addSingleFolder; }
        }

        public ICommand AddRecursiveFolder
        {
            get { return addRecursiveFolder; }
        }

        public ICommand AddUrl
        {
            get { return addUrl; }
        }

        public ICommand RemoveFiles
        {
            get { return removeFiles; }
        }

        public FilesPickerViewModel(IFileDialogConfiguration configuration, ISelectedItemsProvider selectedItemsProvider)
        {
            addFiles = new AddFilesCommand(configuration);
            addSingleFolder = new AddSingleFolderCommand();
            addRecursiveFolder = new AddRecursiveFolderCommand();
            addUrl = new AddUrlCommand();
            removeFiles = new RemoveItemsCommand<string>(selectedItemsProvider);
        }
    }
}
