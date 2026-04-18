import os
import argparse

# Directories to ignore to keep the output clean and under context limits
IGNORE_DIRS = {
    # General
    '.git', 'node_modules', 'venv', '.venv', '__pycache__', 
    'build', 'dist', '.idea', '.vscode', 'coverage',
    
    # Visual Studio / .NET Specific
    '.vs', 'bin', 'obj', 'packages', 'TestResults', 'x64', 'x86',
    
    # Auto-generated EF Core
    'Migrations'
}

# Binary or non-text extensions to skip
IGNORE_EXTS = {
    # General binary/media
    '.png', '.jpg', '.jpeg', '.gif', '.ico', '.pdf', '.zip', '.tar', '.gz',
    '.mp4', '.mp3', '.wav', '.pyc', '.exe', '.dll', '.so', '.dylib', 
    '.class', '.jar', '.woff', '.woff2', '.ttf', '.eot',
    
    # Database and specific generated files
    '.sqlite', '.db', '.sqlite3', '.mdf', '.ldf',
    
    # Build, Object, and IDE Files
    '.pdb', '.cache', '.resources', '.user', '.DotSettings.user'
}

# We can also explicitly skip files with certain names that we know are generated noise
IGNORE_FILES = {
    'packager.py',           # Don't include the packager itself
    'codebase_dump.txt'      # Don't include the output file from previous runs
}

def is_text_file(filepath):
    """Checks if a file is text-based by extension and content."""
    _, ext = os.path.splitext(filepath)
    if ext.lower() in IGNORE_EXTS:
        return False
        
    # Try to open and read a small chunk; if decoding fails, it's likely binary
    try:
        with open(filepath, 'tr', encoding='utf-8') as f:
            f.read(1024)
        return True
    except UnicodeDecodeError:
        return False

def pack_repo(source_dir, output_file):
    """Walks the directory and writes file contents to the output file."""
    with open(output_file, 'w', encoding='utf-8') as outfile:
        for root, dirs, files in os.walk(source_dir):
            
            # Modify dirs in-place to skip ignored directories
            dirs[:] = [d for d in dirs if d not in IGNORE_DIRS]

            for file in files:
                filepath = os.path.join(root, file)
                rel_path = os.path.relpath(filepath, source_dir)

                # Skip hidden files, explicitly ignored files, or specific extensions
                if file.startswith('.') or file in IGNORE_FILES:
                    continue
                
                # Also skip auto-generated .g.cs assembly/UI files in .NET
                if file.endswith('.g.cs') or file.endswith('.g.i.cs') or file.endswith('AssemblyInfo.cs'):
                    print(f"Skipping auto-generated .NET file: {rel_path}")
                    continue

                if not is_text_file(filepath):
                    print(f"Skipping binary/non-text file: {rel_path}")
                    continue

                print(f"Adding: {rel_path}")
                
                # Create a distinct header for each file
                outfile.write(f"\n{'='*80}\n")
                outfile.write(f"File: {rel_path}\n")
                outfile.write(f"{'='*80}\n\n")

                # Write the file contents
                try:
                    with open(filepath, 'r', encoding='utf-8') as infile:
                        outfile.write(infile.read())
                        outfile.write("\n")
                except Exception as e:
                    outfile.write(f"[Error reading file: {e}]\n")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Package a codebase into a single text file for LLM ingestion.")
    parser.add_argument("source_dir", help="Path to the repository directory to package")
    parser.add_argument("-o", "--output", default="codebase_dump.txt", help="Output file name (default: codebase_dump.txt)")

    args = parser.parse_args()

    if not os.path.isdir(args.source_dir):
        print(f"Error: '{args.source_dir}' is not a valid directory.")
        exit(1)

    print(f"Packaging '{args.source_dir}' into '{args.output}'...\n")
    pack_repo(args.source_dir, args.output)
    print("\nDone! Your codebase is ready for upload.")