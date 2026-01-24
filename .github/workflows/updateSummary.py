import argparse
import json
import re
import os

def parse_markdown(markdown):
    lines = markdown.split('\n')
    tree = []
    stack = []

    for line in lines:
        stripped_line = line.strip()
        if re.match(r'^##\s', stripped_line):
            group = {
                'group_name': stripped_line[3:].strip(),
                'pages': []
            }
            tree.append(group)
            stack = [group]
        elif re.match(r'^\*\s', stripped_line):
            page_name = re.search(r'\[(.*?)\]', stripped_line).group(1)
            page_path = re.search(r'\((.*?)\)', stripped_line).group(1)
            page = {
                'page_name': page_name,
                'page_path': page_path,
                'subpages': []
            }
            indent_level = (len(line) - len(stripped_line)) // 2
            while len(stack) > indent_level + 1:
                stack.pop()
            if 'pages' in stack[-1]:
                stack[-1]['pages'].append(page)
            else:
                stack[-1]['subpages'].append(page)
            stack.append(page)
    
    return tree

def serialize_to_markdown(json_content):
    def serialize_group(group):
        markdown = f"## {group['group_name']}\n\n"
        for page in group['pages']:
            markdown += serialize_page(page, 0)
        return markdown + '\n'

    def serialize_page(page, indent_level):
        indent = '  ' * indent_level
        markdown = f"{indent}* [{page['page_name']}]({page['page_path']})\n"
        for subpage in page['subpages']:
            markdown += serialize_page(subpage, indent_level + 1)
        return markdown

    markdown_content = "# Table of contents\n\n"
    for group in json_content:
        markdown_content += serialize_group(group)
    
    return markdown_content

def create_page(file):
    # Get the file name without .md
    parts = file.split('/')
    return {
        'page_name': parts[-1].split('.')[0],
        'page_path': file,
        'subpages': []
    }

def create_category_entry(category_name, category_path, files):
    """Create a category entry with its subpages."""
    # Sort files alphabetically by filename
    sorted_files = sorted(files, key=lambda x: x.split('/')[-1].lower())
    
    return {
        'page_name': category_name,
        'page_path': f'{category_path}/README.md',
        'subpages': [create_page(file) for file in sorted_files if not file.endswith('README.md')]
    }

def get_categories(reference_path):
    """Get all category folders and their files from the reference path."""
    categories = {}
    
    if not os.path.exists(reference_path):
        return categories
    
    for item in os.listdir(reference_path):
        item_path = os.path.join(reference_path, item)
        if os.path.isdir(item_path):
            # Get all .md files in this category folder
            files = [f'reference/{item}/{file}' for file in os.listdir(item_path) if file.endswith('.md')]
            categories[item] = files
    
    return categories

def update_summary(summary, reference_path):
    # Parse the summary markdown content into a JSON tree structure
    tree = parse_markdown(summary)

    print("Parsed tree:", tree)

    # Get all dynamic categories from the reference folder
    categories = get_categories(reference_path)
    print("Found categories:", list(categories.keys()))

    # Find the Reference group
    reference_group = next((group for group in tree if group['group_name'] == 'Reference'), None)
    if reference_group is None:
        print("Warning: Reference group not found in summary")
        return serialize_to_markdown(tree)

    # Preserve callbacks, remove other dynamic categories, and add new ones
    preserved_pages = []
    for page in reference_group['pages']:
        # Keep Callbacks (case-insensitive check)
        if page['page_name'].lower() == 'callbacks':
            preserved_pages.append(page)
    
    # Add dynamic categories (sorted alphabetically)
    for category_name in sorted(categories.keys(), key=str.lower):
        # Skip callbacks as it's preserved
        if category_name.lower() == 'callbacks':
            continue
        
        files = categories[category_name]
        # Create a display name (capitalize first letter)
        display_name = category_name.capitalize() if category_name.islower() else category_name
        category_entry = create_category_entry(display_name, f'reference/{category_name}', files)
        preserved_pages.append(category_entry)
        print(f"Added category: {display_name} with {len(category_entry['subpages'])} subpages")

    # Sort: Callbacks first, then alphabetically
    def sort_key(page):
        if page['page_name'].lower() == 'callbacks':
            return ('0', page['page_name'].lower())
        return ('1', page['page_name'].lower())
    
    reference_group['pages'] = sorted(preserved_pages, key=sort_key)

    # Serialize the JSON tree structure back to markdown content
    return serialize_to_markdown(tree)


if __name__ == "__main__":
    # Parse command line arguments
    parser = argparse.ArgumentParser(description='Update summary.md file with dynamically synced content.')
    parser.add_argument('summary', type=str, help='Path to the summary.md file.')
    parser.add_argument('reference', type=str, help='Path to the reference folder containing category subfolders.')

    args = parser.parse_args()

    # Read the summary.md file
    with open(args.summary, 'r') as file:
        summary = file.read()

    print(f"Reference path: {args.reference}")
    print(f"Categories found: {os.listdir(args.reference) if os.path.exists(args.reference) else 'Path not found'}")

    # Update the summary.md file with the dynamically synced content
    updated_summary = update_summary(summary, args.reference)

    print("\nUpdated summary:")
    print(updated_summary)

    # Write the updated summary.md file
    with open(args.summary, 'w') as file:
        file.write(updated_summary)