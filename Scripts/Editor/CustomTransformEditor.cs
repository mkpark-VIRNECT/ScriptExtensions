#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// トランスフォームのエディタ拡張を行うクラス
/// </summary>
[CustomEditor(typeof(Transform),true)]
public sealed class CustomTransformEditor : Editor
{
    /// <summary>
    /// トランスフォームのデータを表すクラス
    /// </summary>
    private sealed class TransformData
    {
        public Vector3 mPosition;
        public Vector3 mRotation;
        public Vector3 mScale;
    }

    // 変数
    private Transform mTransform;
    private TransformData mLocalData = new TransformData();
    private bool mIsShowingLocal = true;
    private bool mIsShowingGlobal;
    private bool mIsShowingParents = true;


    /// <summary>
    /// インスペクターのGUIを表示するときの処理を行う。
    /// </summary>
    public override void OnInspectorGUI ()
    {
        mTransform = target as Transform;

        GUILayoutHelper.BeginChangeCheck(
            () =>
            {

                OnShowLocalData();
                OnShowGlobalData();

            },
            () =>
            {
                Undo.RecordObject(target, "Undo Transform");

                mTransform.localPosition = mLocalData.mPosition;
                mTransform.localEulerAngles = mLocalData.mRotation;
                mTransform.localScale = mLocalData.mScale;
            }
        );
    }


    /// <summary>
    /// ローカルのデータを表示するときの処理を行う。
    /// </summary>
    private void OnShowLocalData ()
    {
        if (mTransform.parent)
        {
            mIsShowingLocal = EditorGUILayout.Foldout(mIsShowingLocal, "Local");
            if (!mIsShowingLocal) return;
        }

        ShowLocalData(mTransform, mLocalData);
    }


    /// <summary>
    /// ローカルデータを表示する
    /// </summary>
    private void ShowLocalData (Transform transform, TransformData data)
    {
        // 座標
        GUILayoutHelper.BeginHorizontal(() =>
        {
            data.mPosition = transform.localPosition;

            if (GUILayout.Button("Position", GUILayout.MaxWidth(100.0f)))
            {
                data.mPosition = Vector3.zero;
            }

            data.mPosition = EditorGUILayout.Vector3Field("", data.mPosition);
        });

        // 回転
        GUILayoutHelper.BeginHorizontal(() =>
        {
            data.mRotation = transform.localEulerAngles;

            if (GUILayout.Button("Rotation", GUILayout.MaxWidth(100.0f)))
            {
                data.mRotation = Vector3.zero;
            }

            data.mRotation = EditorGUILayout.Vector3Field("", data.mRotation);
        });

        // スケール
        GUILayoutHelper.BeginHorizontal(() =>
        {
            data.mScale = transform.localScale;

            if (GUILayout.Button("Scale", GUILayout.MaxWidth(100.0f)))
            {
                data.mScale = Vector3.one;
            }

            data.mScale = EditorGUILayout.Vector3Field("", data.mScale);
        });
    }


    /// <summary>
    /// グローバルのデータを表示するときの処理を行う。
    /// </summary>
    private void OnShowGlobalData ()
    {
        if (!mTransform.parent) return;

        // Vector3をそのまま文字列化すると、値が大幅に切り捨てられるため
        // 一つずつの要素を文字列化してそれを防ぐ
        System.Func<Vector3, string> toStringFunc = (data) =>
        {
            return "(" + data.x + ", " + data.y + ", " + data.z + ")";
        };

        // トランスフォームデータ
        mIsShowingGlobal = EditorGUILayout.Foldout(mIsShowingGlobal, "Global");
        if (!mIsShowingGlobal) return;

        EditorGUILayout.LabelField("P " + toStringFunc(mTransform.position) + ", "
                                 + "R " + toStringFunc(mTransform.eulerAngles) + ", "
                                 + "S " + toStringFunc(mTransform.lossyScale));

        GUILayoutHelper.BeginHorizontal(() =>
        {
                // タブ代わり
                EditorGUILayout.LabelField("", GUILayout.Width(10.0f));

            GUILayoutHelper.BeginVertical(() =>
            {
                    // 親オブジェクト
                    mIsShowingParents = EditorGUILayout.Foldout(mIsShowingParents, "Parents");
                if (!mIsShowingParents) return;

                    // 毎フレームリストはクリアする
                    ShowParentData(mTransform.parent);
            });
        });
    }


    /// <summary>
    /// 親データの表示をする。
    /// </summary>
    /// <param name="transform">( 親の )トランスフォーム</param>
    private void ShowParentData (Transform transform)
    {
        if (!transform) return;

        EditorGUILayout.LabelField(transform.name);

        TransformData data = new TransformData();

        data.mPosition = transform.localPosition;
        data.mRotation = transform.localEulerAngles;
        data.mScale = transform.localScale;

        ShowLocalData(transform, data);

        transform.localPosition = data.mPosition;
        transform.localEulerAngles = data.mRotation;
        transform.localScale = data.mScale;

        ShowParentData(transform.parent);
    }
}
#endif