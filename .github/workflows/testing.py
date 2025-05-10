import json
import re

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

# Example markdown content
markdown_content = """
# Table of contents

## Custom Map Tutorial

* [Custom Map Introduction](README.md)
* [Your first map](custom-map-tutorial/your-first-map.md)
* [Map Navigation](custom-map-tutorial/map-navigation.md)
* [Object Selection](custom-map-tutorial/object-selection.md)
* [Object Positioning](custom-map-tutorial/object-positioning.md)
* [Object Attributes](custom-map-tutorial/object-attributes.md)
* [Shortcuts and Macros](custom-map-tutorial/shortcuts-and-macros.md)
* [Editor Settings](custom-map-tutorial/editor-settings.md)
* [Built-in Components Common Errors](custom-map-tutorial/built-in-components-common-errors.md)
* [Map Performance](custom-map-tutorial/map-performance.md)
* [Custom Assets](custom-map-tutorial/custom-assets/README.md)
  * [Your first Asset Bundle](custom-map-tutorial/custom-assets/your-first-asset-bundle.md)
  * [Asset Bundles in Map Editor](custom-map-tutorial/custom-assets/asset-bundles-in-map-editor.md)
  * [Asset Bundles in Game](custom-map-tutorial/custom-assets/asset-bundles-in-game.md)
  * [Adding to Asset Bundles](custom-map-tutorial/custom-assets/adding-to-asset-bundles.md)
  * [Asset Bundle naming](custom-map-tutorial/custom-assets/asset-bundle-naming.md)

## Custom Logic Tutorial

* [Custom Logic Introduction](custom-logic-tutorial/custom-logic-introduction.md)
* [Your first script](custom-logic-tutorial/your-first-script.md)
* [Variables](custom-logic-tutorial/variables.md)
* [Types](custom-logic-tutorial/types.md)
* [Variable Inspector](custom-logic-tutorial/variable-inspector.md)
* [Expressions](custom-logic-tutorial/expressions.md)
* [Conditionals](custom-logic-tutorial/conditionals.md)
* [Loops](custom-logic-tutorial/loops.md)
* [Functions](custom-logic-tutorial/functions.md)
* [Coroutines](custom-logic-tutorial/coroutines.md)
* [Classes](custom-logic-tutorial/classes.md)
* [Static Classes](custom-logic-tutorial/static-classes.md)
* [Components](custom-logic-tutorial/components.md)
* [Extensions](custom-logic-tutorial/extensions.md)
* [Cutscenes](custom-logic-tutorial/cutscenes.md)
* [Static Objects](custom-logic-tutorial/static-objects.md)
* [Networking](custom-logic-tutorial/networking.md)
* [Commenting](custom-logic-tutorial/commenting.md)

## Reference

* [Static Classes](reference/static-classes/README.md)
  * [Game](reference/static-classes/game.md)
  * [Network](reference/static-classes/network.md)
  * [Map](reference/static-classes/map.md)
  * [UI](reference/static-classes/ui.md)
  * [Time](reference/static-classes/time.md)
  * [Convert](reference/static-classes/convert.md)
  * [String](reference/static-classes/string.md)
  * [Input](reference/static-classes/input.md)
  * [Math](reference/static-classes/math.md)
  * [Random](reference/static-classes/random.md)
  * [Cutscene](reference/static-classes/cutscene.md)
  * [Camera](reference/static-classes/camera.md)
  * [RoomData](reference/static-classes/roomdata.md)
  * [PersistentData](reference/static-classes/persistentdata.md)
  * [Json](reference/static-classes/json.md)
  * [Physics](reference/static-classes/physics.md)
* [Objects](reference/objects/README.md)
  * [Component](reference/objects/component.md)
  * [Object](reference/objects/object.md)
  * [Character](reference/objects/character.md)
  * [Human](reference/objects/human.md)
  * [Titan](reference/objects/titan.md)
  * [Shifter](reference/objects/shifter.md)
  * [MapObject](reference/objects/mapobject.md)
  * [Transform](reference/objects/transform.md)
  * [Player](reference/objects/player.md)
  * [NetworkView](reference/objects/networkview.md)
  * [Color](reference/objects/color.md)
  * [Vector3](reference/objects/vector3.md)
  * [Quaternion](reference/objects/quaternion.md)
  * [Dict](reference/objects/dict.md)
  * [List](reference/objects/list.md)
  * [Range](reference/objects/range.md)
  * [LineCastHitResult](reference/objects/linecasthitresult.md)
  * [MapTargetable](reference/objects/maptargetable.md)
  * [Random](reference/objects/random.md)
* [Callbacks](reference/callbacks/README.md)
  * [Main](reference/callbacks/main.md)
  * [Components](reference/callbacks/components.md)

## Examples

* [Gamemodes](examples/gamemodes/README.md)
  * [Survive](examples/gamemodes/survive.md)
  * [Waves](examples/gamemodes/waves.md)
  * [Endless](examples/gamemodes/endless.md)
  * [Racing](examples/gamemodes/racing.md)
  * [Blade PVP](examples/gamemodes/blade-pvp.md)
  * [Thunderspear PVP](examples/gamemodes/thunderspear-pvp.md)
  * [Titan Explode](examples/gamemodes/titan-explode.md)
  * [Cranked](examples/gamemodes/cranked.md)
  * [More Examples](examples/gamemodes/more-examples.md)
* [Components](examples/components/README.md)
  * [SupplyStation](examples/components/supplystation.md)
  * [Daylight](examples/components/daylight.md)
  * [PointLight](examples/components/pointlight.md)
  * [Rigidbody](examples/components/rigidbody.md)
  * [NavMeshObstacle](examples/components/navmeshobstacle.md)
  * [Cannon](examples/components/cannon.md)
  * [Dummy](examples/components/dummy.md)
  * [Wagon](examples/components/wagon.md)
  * [Tag](examples/components/tag.md)
  * [KillRegion](examples/components/killregion.md)
  * [DamageRegion](examples/components/damageregion.md)
  * [MovePingPong](examples/components/movepingpong.md)
  * [RacingCheckpointRegion](examples/components/racingcheckpointregion.md)
  * [RacingFinishRegion](examples/components/racingfinishregion.md)
  * [TeleportRegion](examples/components/teleportregion.md)
  * [Animal](examples/components/animal.md)
  * [SignalMover](examples/components/signalmover.md)
  * [SignalSender](examples/components/signalsender.md)
  * [More Examples](examples/components/more-examples.md)
"""

# Parse the markdown content
parsed_content = parse_markdown(markdown_content)

# Update the parsed content

# Convert back to markdown
markdown_content = serialize_to_markdown(parsed_content)
