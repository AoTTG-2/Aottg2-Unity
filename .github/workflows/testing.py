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
    sorted_files = sorted(files, key=lambda x: x.split('/')[-1].lower())
    
    return {
        'page_name': category_name,
        'page_path': f'{category_path}/README.md',
        'subpages': [create_page(file) for file in sorted_files if not file.endswith('README.md')]
    }

def simulate_categories():
    """Simulate dynamic categories for testing."""
    return {
        'callbacks': ['reference/callbacks/main.md', 'reference/callbacks/components.md', 'reference/callbacks/README.md'],
        'Collections': ['reference/Collections/List.md', 'reference/Collections/Dict.md', 'reference/Collections/README.md'],
        'Component': ['reference/Component/Transform.md', 'reference/Component/NetworkView.md', 'reference/Component/README.md'],
        'Entities': ['reference/Entities/Human.md', 'reference/Entities/Titan.md', 'reference/Entities/Character.md', 'reference/Entities/README.md'],
        'Enums': ['reference/Enums/KeyCode.md', 'reference/Enums/README.md'],
        'Game': ['reference/Game/Player.md', 'reference/Game/MapObject.md', 'reference/Game/README.md'],
        'UIElements': ['reference/UIElements/Button.md', 'reference/UIElements/Label.md', 'reference/UIElements/README.md'],
        'Utility': ['reference/Utility/Vector3.md', 'reference/Utility/Color.md', 'reference/Utility/Quaternion.md', 'reference/Utility/README.md'],
    }

def update_summary_with_categories(summary, categories):
    """Update summary with dynamic categories."""
    tree = parse_markdown(summary)
    
    reference_group = next((group for group in tree if group['group_name'] == 'Reference'), None)
    if reference_group is None:
        print("Warning: Reference group not found")
        return serialize_to_markdown(tree)
    
    # Preserve callbacks, add dynamic categories
    preserved_pages = []
    for page in reference_group['pages']:
        if page['page_name'].lower() == 'callbacks':
            preserved_pages.append(page)
    
    for category_name in sorted(categories.keys(), key=str.lower):
        if category_name.lower() == 'callbacks':
            continue
        
        files = categories[category_name]
        display_name = category_name.capitalize() if category_name.islower() else category_name
        category_entry = create_category_entry(display_name, f'reference/{category_name}', files)
        preserved_pages.append(category_entry)
    
    # Sort: Callbacks first, then alphabetically
    def sort_key(page):
        if page['page_name'].lower() == 'callbacks':
            return ('0', page['page_name'].lower())
        return ('1', page['page_name'].lower())
    
    reference_group['pages'] = sorted(preserved_pages, key=sort_key)
    
    return serialize_to_markdown(tree)

# Example markdown content
markdown_content = """
# Table of contents

## Custom Map Tutorial

* [Custom Map Introduction](README.md)
* [Your first map](custom-map-tutorial/your-first-map.md)

## Custom Logic Tutorial

* [Custom Logic Introduction](custom-logic-tutorial/custom-logic-introduction.md)
* [Your first script](custom-logic-tutorial/your-first-script.md)

## Reference

* [Callbacks](reference/callbacks/README.md)
  * [Main](reference/callbacks/main.md)
  * [Components](reference/callbacks/components.md)

## Examples

* [Gamemodes](examples/gamemodes/README.md)
  * [Survive](examples/gamemodes/survive.md)
"""

# Parse the markdown content
parsed_content = parse_markdown(markdown_content)
print("Parsed content:", json.dumps(parsed_content, indent=2))

# Simulate dynamic categories
categories = simulate_categories()
print("\nSimulated categories:", list(categories.keys()))

# Update with dynamic categories
updated_markdown = update_summary_with_categories(markdown_content, categories)
print("\nUpdated markdown:")
print(updated_markdown)
