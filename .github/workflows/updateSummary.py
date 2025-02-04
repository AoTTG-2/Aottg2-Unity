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


def update_summary(summary, object_files, static_files):
    # Parse the summary markdown content into a JSON tree structure
    tree = parse_markdown(summary)

    print(tree)

    # Delete all pages under reference/object and reference/static except for the README.md pages
    reference = tree.index(next(group for group in tree if group['group_name'] == 'Reference'))
    refs = tree[reference]['pages']

    for ref in refs:
        if ref['page_name'] == 'Objects':
            new_pages = [create_page(file) for file in object_files if file != 'reference/objects/README.md']
            ref['subpages'] = new_pages
        if ref['page_name'] == 'Static Classes':
            new_pages = [create_page(file) for file in static_files if file != 'reference/static/README.md']
            ref['subpages'] = new_pages

    # Serialize the JSON tree structure back to markdown content
    return serialize_to_markdown(tree)



if __name__ == "__main__":
    # Parse command line arguments which contains a file path for the summary.md file
    parser = argparse.ArgumentParser(description='Update summary.md file with dynamically synced content.')
    parser.add_argument('summary', type=str, help='Path to the summary.md file.')
    
    # Single folder for object folder
    parser.add_argument('objects', type=str, help='Path to the objects folder.')
    parser.add_argument('static', type=str, help='Path to the static folder.')

    args = parser.parse_args()

    # Read the summary.md file
    with open(args.summary, 'r') as file:
        summary = file.read()

    # Get all file names under the object folder
    object_files = [f'reference/objects/{file}' for file in os.listdir(args.objects)]
    static_files = [f'reference/static/{file}' for file in os.listdir(args.static)]

    # Sort both lists alphabetically by filename
    object_files.sort(key=lambda x: x.split('/')[-1])
    static_files.sort(key=lambda x: x.split('/')[-1])

    # print object_files and static_files
    print("object_files: ", object_files)
    print("static_files: ", static_files)


    # Update the summary.md file with the dynamically synced content
    updated_summary = update_summary(summary, object_files, static_files)

    print(updated_summary)

    # Write the updated summary.md file
    with open(args.summary, 'w') as file:
        file.write(updated_summary)