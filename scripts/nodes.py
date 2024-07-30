#Generates a node class to work with Context-Free Grammars in an AST

def generate_file(definitions: list[str]):
    generated = ""
    for definition in definitions:
        class_name, inheritee, types, argument_names = parse_definition(definition)
        generated += generate_class(class_name, inheritee, types, argument_names) + "\n\n"

    with open("classes", "w") as file:
        file.write(generated)

    print(f"Code written to classes.cs")

def parse_definition(node_def: str):
    node_def = node_def.replace(' < ', '<').replace(' : ', ':')
    class_info = node_def.split(":")[0].strip()
    fields_info = node_def.split(":")[1].strip()
    
    class_name = class_info.split("<")[0].strip()
    inheritee = class_info.split("<")[1].strip()
    
    types = []
    argument_names = []
    segments = fields_info.split(',')
    for segment in segments:
        segment = segment.strip()
        declarations = segment.split()
        types.append(declarations[0])
        argument_names.append(declarations[1])

    return class_name, inheritee, types, argument_names 

def generate_class(classname: str, inherit_from: str, field_types: list[str], fields: list[str]):
    if len(field_types) != len(fields):
        raise ValueError("Number of types must be equal to number of fields")

    class_declaration = f"public class {classname} : {inherit_from} \n{{"

    field_declarations = "\n" + "\n".join(f"\tpublic {ft} {f};" for ft, f in zip(field_types, fields))

    constructor_params = ", ".join(f"{ft} _{f}" for ft, f in zip(field_types, fields))
    constructor_declaration = f"\n\n\t{classname}({constructor_params})"

    constructor_body = "\n".join(f"\t\t{f} = _{f};" for f in fields)
    constructor_body = f"\n\t{{\n{constructor_body}\n\t}}"

    generated = f"{class_declaration}{field_declarations}{constructor_declaration}{constructor_body}\n}}"
    return generated

generate_file([
    "BinaryExpression < Expression : Exprresion left, Token operator, Expression right",
    "UnaryExpression < Expression : Expression expression, Token operator"
])