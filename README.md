# ClientForFetchingMods ![mcsrvrhlpr](https://github.com/user-attachments/assets/b9cd3be0-5275-430e-a88b-c0d115a4a4d3)


A client for fetching minecraft mods off a TrueNas Server that hosts them, to ensure compatability and simplify gathering all required mods for a specific Minecraft Server

**ClientForFetchingMods (CFFM)** is a Windows Forms tool that helps you **download and manage Minecraft mods** from a remote TrueNAS-hosted SFTP share. It securely downloads `.jar` mod files, automatically backs up your existing mods, transfers new ones to your local `.minecraft\mods` folder, and offers a convenient drag-and-drop interface along with comprehensive logging.

![mcsrvrhlpr](https://github.com/user-attachments/assets/0158557d-288d-44d4-9986-34c86527ecd8)

## Features

- **Secure SFTP Connectivity**: Download `.jar` mods from a remote TrueNAS or any SSH-enabled server.
- **One-Click Mod Transfer**: Automatically downloads mods to a temporary folder and then transfers them to your local `.minecraft\mods` folder.
- **Backup Existing Mods**: Existing `.jar` files are automatically moved to a `mods_old` folder before new mods are copied.
- **Responsive Async UI**: All long-running SFTP operations run asynchronously to keep the user interface responsive.
- **Rich Logging Panel**: A built-in log panel displays timestamped actions and events.
- **Drag-and-Drop Support**: Easily drag `.jar` files from the remote list to the local list for individual downloads.
- **Local Mod Management**: Delete selected or all local `.jar` mods with dedicated buttons.
- **Mod Count Indicators**: View the total number of mods found on the server and locally.

## Getting Started

### 1. Download ClientForFetchingMods

1. Visit the [Releases](https://github.com/medenijazbec/ClientForFetchingMods/releases) page.
2. Download the latest **`ClientForFetchingMods.exe`** (precompiled for Windows x64).
3. Place the executable in a folder of your choice (e.g., `C:\MinecraftTools`).

### 2. Run the Application

1. Double-click **`ClientForFetchingMods.exe`**.
2. The application displays two panels:
   - **Server Mods**: Shows the available `.jar` mod files on your remote SFTP server.
   - **Local Mods**: Shows the `.jar` mod files in your local `.minecraft\mods` folder.
3. A log panel at the bottom will show detailed status messages.

### 3. Connect to Your SFTP Server

1. Click the **Connect (SFTP)** button.
2. Enter your SFTP host credentials (Host, Username, and Password).
3. By default, the remote folder is set to `/mnt/Files/currentMods`—change this in the source if needed.

### 4. Download & Transfer Mods

1. Click the **Download and Transfer Mods** button.
2. The application will:
   - Download all `.jar` mods from the SFTP server to a temporary folder.
   - Backup any existing local mods.
   - Transfer the new mods into your local `.minecraft\mods` folder.
3. The progress bar and log panel will update in real time.

### 5. Manage Local Mods

- **Drag-and-Drop**: Drag items from the Server Mods list and drop them into the Local Mods panel for individual downloads.
- **Delete Selected Local Mods**: Remove any selected `.jar` files from your local mods folder.
- **Delete All Local Mods**: Remove all `.jar` files from your local mods folder.
- **Refresh Local Mods**: Refresh only the local mods list.
- **Open Folder**: Quickly open the local `.minecraft\mods` folder in Explorer.

## Building from Source

### Prerequisites

- **.NET 9 SDK**  
  Download and install the [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

- **Visual Studio 2022** or **Visual Studio Code**  
  You can use Visual Studio 2022 (recommended for full IDE support) or VS Code with the C# extension.

### Steps

1. **Clone the Repository**  
   Open a terminal and run:
   ```bash
   git clone https://github.com/medenijazbec/ClientForFetchingMods.git
   cd ClientForFetchingMods

2. **Open the Solution**
   Visual Studio: Open the ClientForFetchingMods.sln solution file.
   Visual Studio Code: Open the repository folder and allow the C# extension to load the project.

3. **Restore Dependencies**
   Visual Studio will automatically restore NuGet packages on build. Alternatively, run:
   `dotnet restore`

4. **Build the Project**
   In Visual Studio, select Build → Build Solution.
   Or from the command line, run:
   `dotnet build -c Release`

5. **Publish the Application**
   To create a single-file, self-contained executable for Windows x64, run:
   `dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true`
   The published output will be located in the `bin/Release/net9.0/win-x64/publish` directory.

   **Troubleshooting**
   If you experience issues with missing dependencies or extra files, ensure that the correct runtime identifier (win-x64) is used and that all NuGet packages are restored.














