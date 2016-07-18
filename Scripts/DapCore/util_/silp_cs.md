# RESPONSE_HELPER_GET_REQ_TYPE(type, cs_type) #
```
public static ${cs_type} GetReq${type}(Data response, string key) {
    Data req = GetReq(response);
    if (req == null) {
        return default(${cs_type});
    }
    return req.Get${type}(key);
}

public static ${cs_type} GetReq${type}(Data response, string key, ${cs_type} defaultValue) {
    Data req = GetReq(response);
    if (req == null) {
        return defaultValue;
    }
    return req.Get${type}(key, defaultValue);
}

```

# RESPONSE_HELPER_GET_RESULT_TYPE(type, cs_type) #
```
public static ${cs_type} GetResult${type}(Data response, string key) {
    Data result = GetResult(response);
    if (result == null) {
        return default(${cs_type});
    }
    return result.Get${type}(key);
}

public static ${cs_type} GetResult${type}(Data response, string key, ${cs_type} defaultValue) {
    Data result = GetResult(response);
    if (result == null) {
        return defaultValue;
    }
    return result.Get${type}(key, defaultValue);
}

```


