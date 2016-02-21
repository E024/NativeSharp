template <typename T> class __array;

template <typename T, typename TUnderlying> struct __enum
{
public: 
	TUnderlying _value;
	__enum() = default;
	inline __enum(TUnderlying value) : _value(value) {}
	inline operator TUnderlying() { return _value; }
	inline __enum& operator++()
	{
		// actual increment takes place here
		_value++;
		return *this;
	}
	inline __enum operator++(int)
	{
		__enum tmp(*this);
		operator++();
		return tmp;
	}
};

template <typename T> struct __unbound_generic_type
{
};

inline void* __new (size_t _size)
{
    auto mem = ::operator new(_size);
    std::memset(mem, 0, _size);
    return mem;
}

template< typename T, typename C>
class __static 
{
	T t;
public:

	inline T& operator=(T value)
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}
	
		t = value;

		return *this;
	}

	inline operator T&()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return t;
	}

	inline T& operator ->()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return t;
	}

	inline T* operator &()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return &t;
	}

	template <typename D, class = typename std::enable_if<std::is_enum<T>::value>> inline explicit operator D()
	{
		if (!C::_cctor_called)
		{
			C::_cctor();
		}

		return (D)t;
	}
};
