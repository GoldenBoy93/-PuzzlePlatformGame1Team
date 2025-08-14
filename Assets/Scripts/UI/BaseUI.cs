using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� UI�� �⺻ ������ �����ϴ� �߻� Ŭ����
public abstract class BaseUI : MonoBehaviour
{
    protected UIManager uiManager;

    // �ʿ��ϴٸ� UIManager �޼��� ȣ���� �����ϰ� �ϱ� ���� �ʱ�ȭ �޼���
    // UIManager���� �� UI�� Init ���� (���⼱ �޴°�)
    // virtual�̴� �ڽ� Ŭ�������� override�� ����
    public virtual void Init(UIManager uiManager)
    {
        this.uiManager = uiManager;
    }
}