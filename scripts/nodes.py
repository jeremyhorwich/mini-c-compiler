#Generates a node class to work with Context-Free Grammars in an AST

def generate(classname: str, inherit_from: str, field_types: list[str], fields: list[str]):
    if len(field_types) != len(fields):
        raise ValueError("Number of types must be equal to number of fields")

    class_declaration = f"public class {classname} : {inherit_from} \n{{"

    field_declarations = "\n" + "\n".join(f"\tpublic {ft} {f};" for ft, f in zip(field_types, fields))

    constructor_params = ", ".join(f"{ft} _{f}" for ft, f in zip(field_types, fields))
    constructor_declaration = f"\n\n\t{classname}({constructor_params})"

    constructor_body = "\n".join(f"\t\t{f} = _{f};" for f in fields)
    constructor_body = f"\n\t{{\n{constructor_body}\n\t}}"

    generated = f"{class_declaration}{field_declarations}{constructor_declaration}{constructor_body}\n}}"

    with open(classname, 'w') as file:
        file.write(generated)

    print(f"Code written to {classname}.cs")

generate("BinaryExpression", "Expression", ["Expression", "Token", "Expression"], ["left", "operator", "right"])