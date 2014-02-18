
xcopy /s Z:\home\swgemu\workspace\SWGEmuRPCServer\rpc\proto .\SWGEmu-API\protogen\proto /Y

FOR /r %%X IN (.\SWGEmu-API\protogen\proto\*.proto) DO (
    .\SWGEmu-API\protogen\proto\ProtoGen.exe "%%X" --include_imports -output_directory="%~dp0.\SWGEmu-API\protogen" --proto_path="%~dp0./SWGEmu-API/protogen/proto" -service_generator_type=GENERIC
)