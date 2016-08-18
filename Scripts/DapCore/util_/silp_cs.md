# REQUEST_HELPER_GET_TYPE(type, cs_type) #
```
public static ${cs_type} Get${type}(Handler handler, Data req, string key, Data hint) {
    if (req == null || !req.Is${type}(key)) {
        throw new BadRequestException(handler, req, hint);
    }
    return req.Get${type}(key);
}

public static ${cs_type} Get${type}(Handler handler, Data req, string key, string format, params object[] values) {
    if (req == null || !req.Is${type}(key)) {
        throw new BadRequestException(handler, req, format, values);
    }
    return req.Get${type}(key);
}

public static ${cs_type} GetReq${type}(Data req, string key, ${cs_type} defaultValue) {
    if (req == null) {
        return defaultValue;
    }
    return req.Get${type}(key, defaultValue);
}

```

# RESPONSE_HELPER_GET_REQ_TYPE(type, cs_type) #
```
public static ${cs_type} GetReq${type}(Data res, string key) {
    Data req = GetReq(res);
    if (req == null) {
        return default(${cs_type});
    }
    return req.Get${type}(key);
}

public static ${cs_type} GetReq${type}(Data res, string key, ${cs_type} defaultValue) {
    Data req = GetReq(res);
    if (req == null) {
        return defaultValue;
    }
    return req.Get${type}(key, defaultValue);
}

```

# RESPONSE_HELPER_GET_RESULT_TYPE(type, cs_type) #
```
public static ${cs_type} GetResult${type}(Data res, string key) {
    Data result = GetResult(res);
    if (result == null) {
        return default(${cs_type});
    }
    return result.Get${type}(key);
}

public static ${cs_type} GetResult${type}(Data res, string key, ${cs_type} defaultValue) {
    Data result = GetResult(res);
    if (result == null) {
        return defaultValue;
    }
    return result.Get${type}(key, defaultValue);
}

```


